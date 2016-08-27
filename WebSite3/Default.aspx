<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script type="text/x-mathjax-config">
        MathJax.Hub.Config({tex2jax: {inlineMath: [['$','$'], ['\\(','\\)']]}});
    </script>
    <script type="text/javascript" async="async"
            src="https://cdn.mathjax.org/mathjax/latest/MathJax.js?config=TeX-AMS_CHTML">
    </script>
    <title></title>
</head>
<body>
    <div style="margin-left: 10%; margin-right: 10%">
        <form id="form1" runat="server">
            <div>
                <asp:TextBox ID="TextBox1" runat="server" Width="770"></asp:TextBox>        
                <asp:Button ID="Button1" runat="server" Text="Решить" OnClick="Button1_Click" Width="230px"/>
            </div>
            <div id="result" runat="server">
            </div>
        </form>
        <div id="history" runat="server">
            <hr /> $$ $$
            <p>История решения математических выражений подобно этому \(\frac{1}{3}\)</p>
            <p>Примеры:</p>
            <table style="width: 100%; border-collapse: collapse;">
                <tr style="text-align: center; border: 2px solid slategray;">
                    <td>2^3+8*(7/5)</td>
                    <td>9*(11-8+4x)*sin(7/(2x-5))</td>
                </tr>
                <tr style="border: 2px solid slategray;">
                    <td>$$2^3+8\cdot\frac{7}{5}$$</td>
                    <td>$$9\cdot \left(11 -8 + 4 \cdot x \right)\cdot sin \left(\frac{7}{2\cdot x-5}\right)$$</td>
                </tr>
            </table>
            <hr />
        </div>
    </div>
</body>
</html>
