Библиотека FunctionParser
версия 1.0

copyRight() Анна Чередник 23.04.2016 (27.08.2016 изменения и исправления)

Реализованные возможности:

class Expression			-	класс, содержащий строку-выражение для разбора

class Function				-	виртуальный класс-родитель, содержащий дерево разбора матем. выражения

class BracketFunction		-	потомок Function, отображающий скобки в матем. выражении
								общий вид: (Function)

class StandartFunction		-	потомок Function, отображающий стандартные матем. функции
								общий вид: {название_функции}(Function)

class TwoOperandFunction	-	потомок StandartFunction, отобращающий бинарные матем. операции
								общий вид: Function{операция}Function

Дочерние от TwoOperandFunction классы - SumFunction, MinusFunction, Multiple, Fraction, Power

class Variable				-	потомок Function, отображающий переменную, принимающую любое численное значение, в матем. выражении

class Constant				-	потомок Variable, отображающий константу (число или известную константу) в матем. выражении
примечание: в будущем разбить данный класс на: одноименный(для констант), Number(для чисел), Complex(комплексных чисел) и возможно другие виды констант

Дочерние от Constant классы - NameConstant, Number