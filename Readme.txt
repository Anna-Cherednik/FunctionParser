���������� FunctionParser
������ 1.0

copyRight() ���� �������� 23.04.2016 (27.08.2016 ��������� � �����������)

������������� �����������:

class Expression			-	�����, ���������� ������-��������� ��� �������

class Function				-	����������� �����-��������, ���������� ������ ������� �����. ���������

class BracketFunction		-	������� Function, ������������ ������ � �����. ���������
								����� ���: (Function)

class StandartFunction		-	������� Function, ������������ ����������� �����. �������
								����� ���: {��������_�������}(Function)

class TwoOperandFunction	-	������� StandartFunction, ������������ �������� �����. ��������
								����� ���: Function{��������}Function

�������� �� TwoOperandFunction ������ - SumFunction, MinusFunction, Multiple, Fraction, Power

class Variable				-	������� Function, ������������ ����������, ����������� ����� ��������� ��������, � �����. ���������

class Constant				-	������� Variable, ������������ ��������� (����� ��� ��������� ���������) � �����. ���������
����������: � ������� ������� ������ ����� ��: �����������(��� ��������), Number(��� �����), Complex(����������� �����) � �������� ������ ���� ��������

�������� �� Constant ������ - NameConstant, Number