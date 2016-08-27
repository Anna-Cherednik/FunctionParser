using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionParser
{
    // перечисление, содержащее тип токенов при синтаксическом разборе строки
    public enum term { none, number, negotive, whitespace, letter, multiline, leftbracket, rightbracket, operation };
    // перечисление, содержащее названия стандартных математических функций
    public enum namefunc  { sin, cos, asin, acos, tg, tan, ctg, ctan, sinh, cosh, tgh, tanh, ctgh, ctanh, ln, log, lg, exp}
    // перечисление, содержащее названия именованных математических констант
    public enum nameconst { Pi, pi, Eps, E, e }

    // класс Токен, содержащий строковое значение токена из синтаксического разбора и его тип
    public class Token
    {
        public string value { set; get; }
        public term type { set; get;  }
    }

    // класс Выражение, который на основании строки формирует список токенов синтаксического разбора,
    // и на основании их - дерево выражения. И позволяет выполнять над ним заданные операции
    // Calculate() - вернуть численное значение матем. выражения, если это возможно
    // Simplify() - вернуть строковое проедставление упроженного матем. выражения от исходного 
    public class Expression
    {
        public Function tree;

        public List<Token> terms;

        public Expression()
        {
            this.tree = null;
            this.terms = new List<Token>();
        }

        public Expression(string expression)
        {
            lexemAnalyse(expression);
            tree = buildTree();
        }

        public override string ToString()
        {
            string str = "(";
            for (int i=0;i<terms.Count;i++)
            {
                str += terms[i].value + ": " + terms[i].type + "\r\n";
            }
            str += ")";
            return str;
        }

        public string ToLatex()
        {
            // Для корректного отображения ошибки предусмотреть выполнение условия: наличие ключевого слова error вначале строки
			string str = "$$" + this.tree.ToLatex() + "$$";
            return str;
        }

        public void lexemAnalyse(string expression)
        {
            string token = "";
            Token tok;
            term currentTerm = term.none;
            this.terms = new List<Token>();

            for (int i = 0; i < expression.Length; i++)
            {
                if (Char.IsNumber(expression[i]))  //IsDigit
                {
                    if ((currentTerm == term.negotive) || (currentTerm == term.number))
                    {
                        token += expression[i];
                        currentTerm = term.number;
                    }
                    else if ((currentTerm != term.none) && (currentTerm != term.whitespace))
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.number;
                    }
                    else
                    {
                        token = expression[i].ToString();
                        currentTerm = term.number;
                    }
                }
                else if (Char.IsLetter(expression[i]))
                {
                    if (currentTerm == term.number)
                    {
                        //this.terms.Add("*");
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        tok = new Token();
                        tok.value = "*";
                        tok.type = term.operation;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.letter;
                    }
                    else if ((currentTerm == term.letter) || (currentTerm == term.multiline))
                    {
                        token += expression[i].ToString();
                        currentTerm = term.multiline;
                    }
                    else if ((currentTerm == term.leftbracket) || (currentTerm == term.operation))
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.letter;
                    }
                    else if (currentTerm == term.rightbracket)
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        tok = new Token();
                        tok.value = "*";
                        tok.type = term.operation;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.letter;
                    }
                    else if ((currentTerm == term.none) || (currentTerm == term.whitespace))
                    {
                        token = expression[i].ToString();
                        currentTerm = term.letter;
                    }
                    else
                    {
                        token += expression[i];
                    }
                }
                else if (Char.IsWhiteSpace(expression[i]))
                {
                    if ((currentTerm != (int)term.none) && (currentTerm != term.whitespace))
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = "";
                        currentTerm = term.whitespace;
                    }

                }
                else if (Char.IsSymbol(expression[i]) || (expression[i] == '*') || (expression[i] == '/'))
                {
                    if ((currentTerm == term.number) || (currentTerm == term.letter) ||  (currentTerm == term.rightbracket) || (currentTerm == term.multiline))
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.operation;

                    }
                    else
                    {
                        token = expression[i].ToString();
                        currentTerm = term.operation;
                    }

                }
                else if (expression[i] == '-')
                {
                    if ((currentTerm == term.number) || (currentTerm == term.letter) || (currentTerm == term.rightbracket))

                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.operation;

                    }
                    else if (currentTerm == term.leftbracket)
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.negotive;
                    }
                    else if (currentTerm != (int)term.none)
                    {
                        token = expression[i].ToString();
                        currentTerm = term.operation;
                    }
                    else
                    {
                        token = expression[i].ToString();
                        currentTerm = term.negotive;
                    }

                }
                else if (expression[i] == ',')
                {
                    if (currentTerm == term.number) 

                    {
                        token += expression[i].ToString();

                    }
                }
                else if (expression[i] == '(')
                {
                    if ((currentTerm == term.number) || (currentTerm == term.letter))


                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        tok = new Token();
                        tok.value = "*";
                        tok.type = term.operation;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.leftbracket;

                    }
                    else if ((currentTerm == term.operation) || (currentTerm == term.multiline))
                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.leftbracket;

                    }
                    else if ((currentTerm == term.none) || (currentTerm == term.whitespace))
                    {
                        token = expression[i].ToString();
                        currentTerm = term.leftbracket;

                    }
                }
                else if (expression[i] == ')')
                {
                    if ((currentTerm == term.number) || (currentTerm == term.letter) || (currentTerm == term.rightbracket))

                    {
                        tok = new Token();
                        tok.value = token;
                        tok.type = currentTerm;
                        this.terms.Add(tok);
                        token = expression[i].ToString();
                        currentTerm = term.rightbracket;

                    }
                }
                //else
                //    this.terms.Add(token);
            }
            tok = new Token();
            tok.value = token;
            tok.type = currentTerm;
            this.terms.Add(tok);
        }

        private bool isLessEqualPriority(string currentOperation, string nextOperation)
        {
            switch (currentOperation)
            {
                case "^":
                    //if (nextOperation == "^") return false;  // не проверено, как должно работать (именно х^2^3)
                    //else return true;
                    return true;
                case "*":
                    if ((nextOperation == "^") || (nextOperation == "/"))
                        return false;
                    else return true;
                case "/":
                    if (nextOperation == "^") return false;
                    else return true;
                case "+": case "-":
                    if ((nextOperation == "^") || (nextOperation == "*") || (nextOperation == "/") )
                        return false;
                    else return true;
                default:
                    return true;
            }
        }

        private TwoOperandFunction createOperFunction(string operation, Function right, Function left)
        {
            switch (operation)
            {
                case "*":
                    return new Multiple(operation, right, left);
                case "/":
                    return new Fraction(operation, right, left);
                case "+":
                    return new SummFunction(operation, right, left);
                case "-":
                    return new MinusFunction(operation, right, left);
                case "^":
                    return new Power(operation, right, left);
                default:
                    return new TwoOperandFunction(operation, right, left);
            }
        }

        public Function buildTree()
        {
            ArrayList list = new ArrayList();
            for (int i=0; i<terms.Count;i++)
            {
                if (terms[i].type == term.number)
                {
                    Number c = new Number(terms[i].value);
                    list.Add(c); 
                }
                else if (terms[i].type == term.letter)
                {
                    if (Enum.IsDefined(typeof(nameconst), terms[i].value))
                    {
                        NameConstant v = new NameConstant(terms[i].value);
                        list.Add(v);
                    }
                    else
                    {
                        Variable v = new Variable(terms[i].value);
                        list.Add(v);
                    }
                }
                else if (terms[i].type == term.multiline)
                {
                    if (Enum.IsDefined(typeof(namefunc), terms[i].value) )
                    {
                        list.Add(terms[i]);
                    }
                    else if (Enum.IsDefined(typeof(nameconst), terms[i].value) )
                    {
                        NameConstant v = new NameConstant(terms[i].value);
                        list.Add(v);
                    }
                    else
                    {
                        Variable w = new Variable(terms[i].value[0].ToString());
                        list.Add(w);
                        for (int j = 1; j < terms[i].value.Length; j++)
                        {
                            Token t = new Token();
                            t.value = "*";
                            t.type = term.operation;
                            list.Add(t);
                            Variable k = new Variable(terms[i].value[j].ToString());
                            list.Add(k);
                        }
                    }
                    
                }
                else
                {
                    list.Add(terms[i]);
                }
            }

            int lengthList = list.Count;

            string error = "";
            while (list.Count > 1)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if ((list[i] is Token) && (list.Count > i + 2))
                    {
                        if (((String)((Token)list[i]).value == "(") && (((String)((Token)list[i + 2]).value == ")")))
                        {
                            if (list[i + 1] is Function)
                            {
                                BracketFunction bracket = new BracketFunction((Function)list[i + 1]);
                                list[i] = bracket;
                                list.RemoveAt(i + 1);
                                list.RemoveAt(i + 1);
                                i -= 1;
                                if ((i >= 0) && (Enum.IsDefined(typeof(namefunc), (string)((Token)list[i]).value)))
                                {
                                    StandartFunction func = new StandartFunction((String)((Token)list[i]).value, (Function)list[i + 1]);
                                    list[i] = func;
                                    list.RemoveAt(i + 1);
                                }
                            }
                            else
                            {
                                error = "Встречен знак {(,)}, но список токенов не содержит выражение вида (Function)";
                                ErrorFunction err = new ErrorFunction(error);
                                list.Clear();
                                list.Add(err);
                            }
                        }
                        else if ((String)((Token)list[i]).value == "(")
                            error = "Встречен знак (, но список токенов не содержит выражение вида (Function)";
                    }
                    else error = error = "Встречен неверный токен... ";
                }
                for (int i = 1; i < list.Count; i++)  // instead i=0;
                {
                    if ((list[i] is Token) && (list.Count > i + 1))
                    {
                        if ((String)((Token)list[i]).value == "^")
                        {
                            if ((list[i - 1] is Function) && (list[i + 1] is Function))
                            {
                                if ((list.Count > i + 2) && (list[i + 2] is Token))
                                    if (!isLessEqualPriority((String)((Token)list[i]).value, (String)((Token)list[i + 2]).value))
                                        continue;
                                //TwoOperandFunction two = new TwoOperandFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                TwoOperandFunction two = createOperFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                list[i] = two;
                                list.RemoveAt(i - 1);
                                list.RemoveAt(i);
                                i -= 1;
                            }
                            else error = "Встречен знак ^, но список токенов не содержит выражение вида Function^Function";
                        }
                        else if (((String)((Token)list[i]).value == "*") || (((String)((Token)list[i]).value == "/")))
                        {
                            if ((list[i - 1] is Function) && (list[i + 1] is Function))
                            {
                                if ((list.Count > i + 2) && (list[i + 2] is Token))
                                    if (!isLessEqualPriority((String)((Token)list[i]).value, (String)((Token)list[i + 2]).value))
                                        continue;
                                //TwoOperandFunction two = new TwoOperandFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                TwoOperandFunction two = createOperFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                list[i] = two;
                                list.RemoveAt(i - 1);
                                list.RemoveAt(i);
                                i -= 1;
                            }
                            else error = "Встречен знак {*,/}, но список токенов не содержит выражение вида Function{*,/}Function";
                        }
                        else if (((String)((Token)list[i]).value == "+") || (((String)((Token)list[i]).value == "-")))
                        {
                            if ((list[i - 1] is Function) && (list[i + 1] is Function))
                            {
                                if ((list.Count > i + 2) && (list[i + 2] is Token))
                                    if (!isLessEqualPriority((String)((Token)list[i]).value, (String)((Token)list[i + 2]).value))
                                        continue;
                                //TwoOperandFunction two = new TwoOperandFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                TwoOperandFunction two = createOperFunction((String)((Token)list[i]).value, (Function)list[i + 1], (Function)list[i - 1]);
                                list[i] = two;
                                list.RemoveAt(i - 1);
                                list.RemoveAt(i);
                                i -= 1;
                            }
                            else error = "Встречен знак {+,-}, но список токенов не содержит выражение вида Function{+,-}Function";
                        }
                    }
                    else error = "Встречен неверный токен... ";
                }               
                for (int i = 0; i < list.Count; i++)
                {
                    if ((list[i] is Token) && (list.Count > i + 1))
                    {
                        if (Enum.IsDefined(typeof(namefunc), (string)((Token)list[i]).value))
                        {
                            if (list[i + 1] is BracketFunction)
                            {
                                StandartFunction name = new StandartFunction((String)((Token)list[i]).value, (Function)list[i + 1]);
                                list[i] = name;
                                list.RemoveAt(i + 1);
                            }
                            else error = "Встречен токен " + (String)((Token)list[i]).value + " , но список токенов не содержит выражение вида " + (String)((Token)list[i]).value + "(Function)";
                        }
                    }
                    else error = "Встречен неверный токен... ";
                }
                if (lengthList != list.Count)
                {
                    lengthList = list.Count;
                }
                else
                {
                    //Console.WriteLine("Цикл стремится к бесконечности...");
                    //Console.WriteLine(error);
                    break;
                }

            }

            //Function ff = new Function();
            if (list[0] is Function && list.Count == 1)
                this.tree = (Function)list[0];
            else
                this.tree = new ErrorFunction(error);
            return this.tree;
        }

        public double Calculate()
        {
            if (this.tree != null)
                return this.tree.Evaluate();
            else
                return Double.PositiveInfinity;
        }

        public Function Simplify()
        {
            return this.tree.RollUp();
        }
    }

    // 
    public class Function
    {
        public string expr;
        public double value;

        public virtual double Evaluate()
        {
               return value;
        }

        public override string ToString()
        {
            return this.expr;
        }

        public virtual bool hasNodeFunction()
        {
            return false;
        }

        public virtual Function RollUp()
        {
            return new Function();
        }

        public virtual string ToLatex()
        {
            return this.expr;
        }
    }

    public class ErrorFunction: Function
    {

        public ErrorFunction(string error)
        {
            this.expr = error;
            this.value = Double.NaN;
        }       

        public override Function RollUp()
        {
            return new ErrorFunction(this.expr);
        }

        public override string ToLatex()
        {
            return "$$" + this.expr + "$$";
        }
    }

    public class BracketFunction : Function
    {
        public Function innerNode;

        public BracketFunction(Function f)
        {
            this.innerNode = f;
        }

        public override double Evaluate()
        {
            this.value = this.innerNode.Evaluate();
            return this.value;
        }

        public override string ToString()
        {
            return "(" + this.innerNode.ToString() + ")";
        }

        public override bool hasNodeFunction()
        {
            return true;
        }

        public override Function RollUp()
        {
            this.innerNode = this.innerNode.RollUp();

            if (this.innerNode is Number)
            //if (this.innerNode.GetType() == typeof(Constant))
            {
                //Constant newnode = new Constant(this.innerNode.Evaluate().ToString());
                return this.innerNode;//newnode;
            }
            return new BracketFunction(innerNode);
        }

        public override string ToLatex()
        {
            return "\\left(" + this.innerNode.ToLatex() + "\\right)";
        }
    }

    public class StandartFunction : Function
    {
        public string operation;
        public Function rightNode;

        public StandartFunction(string oper, Function f)
        {
            this.operation = oper;
            this.rightNode = f;
        }

        public override double Evaluate()
        {
            switch (this.operation)
            {
                case "cos":
                    this.value = Math.Cos(this.rightNode.Evaluate());
                    break;
                case "sin":
                    this.value = Math.Sin(this.rightNode.Evaluate());
                    break;
                case "acos":
                    this.value = Math.Acos(this.rightNode.Evaluate());
                    break;
                case "asin":
                    this.value = Math.Asin(this.rightNode.Evaluate());
                    break;
                case "tg": case "tan":
                    this.value = Math.Tan(this.rightNode.Evaluate());
                    break;
                case "ctg": case "ctan":
                    this.value = 1 / Math.Tan(this.rightNode.Evaluate());
                    break;
                case "cosh":
                    this.value = Math.Cosh(this.rightNode.Evaluate());
                    break;
                case "sinh":
                    this.value = Math.Sinh(this.rightNode.Evaluate());
                    break;
           //     case "acosh":
           //         this.value = Math.Acosh(this.rightNode.Evaluate());
          //          break;
          //      case "asinh":
          //          this.value = Math.Asin(this.rightNode.Evaluate());
          //          break;
                case "tgh": case "tanh":
                    this.value = Math.Tanh(this.rightNode.Evaluate());
                    break;
                //      case "ctgh": case "ctanh":
                //          this.value = 1 / Math.Tanh(this.rightNode.Evaluate());
                //          break;
                case "ln":
                    this.value = Math.Log(this.rightNode.Evaluate());
                    break;
                case "lg": case "log":
                    this.value = Math.Log10(this.rightNode.Evaluate());
                    break;
                case "exp":
                    this.value = Math.Exp(this.rightNode.Evaluate());
                    break;
                default:
                    this.value = Double.NaN;
                    break;
            }

            return this.value;
        }

        public override string ToString()
        {
            return this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return this.operation + this.rightNode.ToLatex();
        }

        public override bool hasNodeFunction()
        {
            return true;
        }

        public override Function RollUp()
        {
            this.rightNode = this.rightNode.RollUp();

            if (this.rightNode is Number)
            //if (this.rightNode.GetType() == typeof(Constant))
            {
                Number newnode = new Number(this.Evaluate().ToString());
                return newnode;
            }
            return new StandartFunction(operation, rightNode);
        }
    }

    public class TwoOperandFunction : StandartFunction
    {
        public Function leftNode;

        public TwoOperandFunction(string oper, Function f, Function fu) : base(oper, f)
        {
            this.leftNode = fu;
        }

        public override double Evaluate()
        {
            this.value = Double.NaN;
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return this.leftNode.ToLatex() + this.operation + this.rightNode.ToLatex();
        }

        public override bool hasNodeFunction()
        {
            return true;
        }

        public override Function RollUp()
        {
            this.rightNode = this.rightNode.RollUp();
            this.leftNode = this.leftNode.RollUp();

            if ((this.leftNode is Number) && (this.rightNode is Number))
            //if ((this.leftNode.GetType() == typeof(Constant)) && (this.rightNode.GetType() == typeof(Constant)))
            {
                Number newnode = new Number(this.Evaluate().ToString());
                return newnode;
            }
            else
            {
                TwoOperandFunction newnode;
                switch (operation)
                {
                    case "*":
                        return newnode = new Multiple(operation, rightNode, leftNode);
                    case "/":
                        return newnode = new Fraction(operation, rightNode, leftNode);
                    case "+":
                        return newnode = new SummFunction(operation, rightNode, leftNode);
                    case "-":
                        return newnode = new MinusFunction(operation, rightNode, leftNode);
                    case "^":
                        return newnode = new Power(operation, rightNode, leftNode);
                    default:
                        return newnode = new TwoOperandFunction(operation, rightNode, leftNode);                        
                }
            }
            //return null;
        }
    }

    public class SummFunction : TwoOperandFunction
    {

        public SummFunction(string oper, Function f, Function fu) : base(oper, f, fu)
        {        }

        public override double Evaluate()
        {
            this.value = this.leftNode.Evaluate() + this.rightNode.Evaluate();
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return this.leftNode.ToLatex() + this.operation + this.rightNode.ToLatex();
        }
    }

    public class MinusFunction : TwoOperandFunction
    {

        public MinusFunction(string oper, Function f, Function fu) : base(oper, f, fu)
        { }

        public override double Evaluate()
        {
            this.value = this.leftNode.Evaluate() - this.rightNode.Evaluate();
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return this.leftNode.ToLatex() + this.operation + this.rightNode.ToLatex();
        }
    }

    public class Multiple : TwoOperandFunction
    {

        public Multiple(string oper, Function f, Function fu) : base(oper, f, fu)
        { }

        public override double Evaluate()
        {
            this.value = this.leftNode.Evaluate() * this.rightNode.Evaluate();
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return this.leftNode.ToLatex() + "\\cdot " + this.rightNode.ToLatex();
        }
    }

    public class Fraction : TwoOperandFunction
    {

        public Fraction(string oper, Function f, Function fu) : base(oper, f, fu)
        { }

        public override double Evaluate()
        {
            this.value = this.leftNode.Evaluate() / this.rightNode.Evaluate();
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return "\\frac{" + this.leftNode.ToLatex() + "}{" + this.rightNode.ToLatex() + "}";
        }
    }

    public class Power : TwoOperandFunction
    {

        public Power(string oper, Function f, Function fu) : base(oper, f, fu)
        { }

        public override double Evaluate()
        {
            this.value = Math.Pow(this.leftNode.Evaluate(), this.rightNode.Evaluate());
            return this.value;
        }

        public override string ToString()
        {
            return this.leftNode.ToString() + this.operation + this.rightNode.ToString();
        }

        public override string ToLatex()
        {
            return "{" + this.leftNode.ToLatex() + "}^{" + this.rightNode.ToLatex() + "}";
        }
    }

    public class Variable : Function
    {

        public Variable(string val)
        {
            this.expr = val;
            this.value = Double.NaN;
        }

        public override string ToString()
        {
            return this.expr;
        }

        public override bool hasNodeFunction()
        {
            return false;
        }

        public override Function RollUp()
        {
            return new Variable(expr);
        }
    }

    public class Constant : Variable
    {
        public Constant(string val) : base(val)
        {
            if (!Double.TryParse(val, out this.value))
            {
                switch (val)
                {
                    case "Pi":  case "pi":
                        this.expr = "pi";
                        this.value = Math.PI;
                        break;
                    case "Eps": case "E":   case "e":
                        this.expr = "e";
                        this.value = Math.E;
                        break;
                    default:
                        this.value = Double.NaN;
                        break;
                }
            }
        }

        public override double Evaluate()
        {
            return this.value;
        }

        public override string ToString()
        {
            return this.expr;
        }

        public override bool hasNodeFunction()
        {
            return false;
        }

        public override Function RollUp()
        {
            return new Constant(expr);
        }
    }

    public class NameConstant : Constant
    {
        public NameConstant(string val) : base(val) {  }

        public override Function RollUp()
        {
            return new NameConstant(expr);
        }
    }

    public class Number: Constant
    {
        public Number(string val) : base(val) { }

        public override Function RollUp()
        {
            return new  Number(expr);
        }
    }
}
