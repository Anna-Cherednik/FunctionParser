using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FunctionParser;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        Expression func = new Expression(TextBox1.Text);

        history.InnerHtml += result.InnerHtml;

        result.InnerHtml = "----------------------------------------------------------- $$ $$";
        result.InnerHtml += "Решение: " + func.ToLatex();
        result.InnerHtml += "$$ = " + Convert.ToString(func.Calculate()) + "$$";
        result.InnerHtml += "\nУпрощение: ";

        result.InnerHtml += "$$ = " + func.Simplify().ToLatex() + "$$"; // func.ToLatex();
    }
}