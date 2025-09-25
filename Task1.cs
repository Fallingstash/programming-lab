using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace labs_programming {
  public partial class Task1 : Form {
    TextBox txtA, txtB, txtE, txtFunction;
    Button btnCalculate;
    Label lblResult;
    PictureBox graphBox;

    public Task1() {
      InitializeComponent();
      this.Text = "Поиск минимума функции";
      this.Size = new Size(800, 800);
      this.StartPosition = FormStartPosition.CenterScreen;
      CreateControls();
    }


    private void CreateControls() {
      int yPos = 10; 

      Label lblFunction = new Label() {
        Text = "Функция f(x):",
        Location = new Point(10, yPos),
        Size = new Size(100, 20)
      };
      txtFunction = new TextBox() {
        Text = "x*x - 2*x + 1", // Функция по умолчанию
        Location = new Point(120, yPos),
        Size = new Size(200, 20)
      };
      this.Controls.Add(lblFunction); 
      this.Controls.Add(txtFunction);
      yPos += 30;

      Label lblA = new Label() {
        Text = "Левая граница a:",
        Location = new Point(10, yPos),
        Size = new Size(100, 20)
      };
      txtA = new TextBox() {
        Text = "-1",
        Location = new Point(120, yPos),
        Size = new Size(60, 20)
      };
      this.Controls.Add(lblA);
      this.Controls.Add(txtA);
      yPos += 30;

      Label lblB = new Label() {
        Text = "Правая граница b:",
        Location = new Point(10, yPos),
        Size = new Size(100, 20)
      };
      txtB = new TextBox() {
        Text = "3",
        Location = new Point(120, yPos),
        Size = new Size(60, 20)
      };
      this.Controls.Add(lblB);
      this.Controls.Add(txtB);
      yPos += 30;

      Label lblE = new Label() {
        Text = "Точность e:",
        Location = new Point(10, yPos),
        Size = new Size(100, 20)
      };
      txtE = new TextBox() {
        Text = "0.001",
        Location = new Point(120, yPos),
        Size = new Size(60, 20)
      };
      this.Controls.Add(lblE);
      this.Controls.Add(txtE);
      yPos += 40;

      btnCalculate = new Button() {
        Text = "Рассчитать",
        Location = new Point(10, yPos),
        Size = new Size(100, 30)
      };
      btnCalculate.Click += Calculate_Click;
      this.Controls.Add(btnCalculate);
      yPos += 40;

      lblResult = new Label() {
        Text = "Результат: ",
        Location = new Point(10, yPos),
        Size = new Size(400, 20)
      };
      this.Controls.Add(lblResult);
      yPos += 30;

      graphBox = new PictureBox() {
        Location = new Point(10, yPos),
        Size = new Size(650, 400),
        BorderStyle = BorderStyle.FixedSingle,
        BackColor = Color.White
      };
      this.Controls.Add(graphBox);
    }

    private double CalculateFunction(double x, string function) {
      string expression = function.Replace("x", x.ToString(CultureInfo.InvariantCulture));
      expression = expression.Replace(" ", "");

      return Convert.ToDouble(new System.Data.DataTable().Compute(expression, ""));
    }

    private double FindMinimum(double a, double b, double e, string function) {
      int iterations = 0; 
      int maxIterations = 1000; // Максимальное число итераций (защита от бесконечного цикла)

      while (b - a > e && iterations < maxIterations) {
        iterations++; 

        double x1 = (a + b) / 2 - e / 2;
        double x2 = (a + b) / 2 + e / 2;

        double f1 = CalculateFunction(x1, function);
        double f2 = CalculateFunction(x2, function);

        if (f1 < f2)
          b = x2;
        else
          a = x1;
      }

      return (a + b) / 2;
    }

    private void Calculate_Click(object sender, EventArgs e) {
      try {
        double a = double.Parse(txtA.Text.Replace(",", "."), CultureInfo.InvariantCulture);
        double b = double.Parse(txtB.Text.Replace(",", "."), CultureInfo.InvariantCulture);
        double epsilon = double.Parse(txtE.Text.Replace(",", "."), CultureInfo.InvariantCulture);
        string function = txtFunction.Text;

        if (a >= b) {
          MessageBox.Show("Левая граница должна быть меньше правой");
          return; // Прерываем выполнение если ошибка
        }

        if (epsilon <= 0) {
          MessageBox.Show("Точность должна быть положительным числом");
          return;
        }

        double minX = FindMinimum(a, b, epsilon, function);
        double minY = CalculateFunction(minX, function);

        lblResult.Text = $"Минимум: x = {minX:F6}, y = {minY:F6}";

        DrawGraph(a, b, function, minX, minY);
      }
      catch (Exception ex) {
        MessageBox.Show($"Ошибка: {ex.Message}");
      }
    }

    private void DrawGraph(double a, double b, string function, double minX, double minY) {
      Bitmap bmp = new Bitmap(graphBox.Width, graphBox.Height);

      using (Graphics g = Graphics.FromImage(bmp)) {
        g.Clear(Color.White); // Заливаем фон белым цветом

        int margin = 50;
        int width = bmp.Width - 2 * margin; 
        int height = bmp.Height - 2 * margin; 

        double minVal = double.MaxValue;
        double maxVal = double.MinValue;

        for (int i = 0; i < 100; i++) {
          double x = a + (b - a) * i / 99; 
          double y = CalculateFunction(x, function);

          if (y < minVal)
            minVal = y;
          if (y > maxVal)
            maxVal = y;
        }

        double yRange = maxVal - minVal;
        if (yRange == 0)
          yRange = 1; 
        minVal -= yRange * 0.1;
        maxVal += yRange * 0.1;
        yRange = maxVal - minVal;

        int ToPixelX(double x) {
          return margin + (int)((x - a) / (b - a) * width);
        }

        int ToPixelY(double y) {
          return margin + height - (int)((y - minVal) / yRange * height);
        }

        Pen axisPen = new Pen(Color.Black, 2);

        int xAxisY = ToPixelY(0);
        g.DrawLine(axisPen, margin, xAxisY, margin + width, xAxisY);

        int yAxisX = ToPixelX(0);
        g.DrawLine(axisPen, yAxisX, margin, yAxisX, margin + height);

        Pen graphPen = new Pen(Color.Blue, 2);
        Point? lastPoint = null;

        for (int i = 0; i < width; i++) {
          double x = a + (b - a) * i / (width - 1);
          double y = CalculateFunction(x, function);

          int pixelX = ToPixelX(x); // Преобразуем x в пиксели
          int pixelY = ToPixelY(y); // Преобразуем y в пиксели

          // Если есть предыдущая точка, рисуем линию к текущей
          if (lastPoint.HasValue) {
            g.DrawLine(graphPen, lastPoint.Value, new Point(pixelX, pixelY));
          }
          lastPoint = new Point(pixelX, pixelY); // Запоминаем текущую точку
        }

        // Рисуем границы a и b зелеными линиями
        int aPixelX = ToPixelX(a);
        int bPixelX = ToPixelX(b);

        g.DrawLine(Pens.Green, aPixelX, margin, aPixelX, margin + height);
        g.DrawLine(Pens.Green, bPixelX, margin, bPixelX, margin + height);

        // Подписываем границы
        g.DrawString($"a = {a:F2}", new Font("Arial", 8), Brushes.Green, aPixelX - 20, margin + height + 5);
        g.DrawString($"b = {b:F2}", new Font("Arial", 8), Brushes.Green, bPixelX - 20, margin + height + 5);

        // Рисуем точку минимума красным кружком
        int minPixelX = ToPixelX(minX);
        int minPixelY = ToPixelY(minY);

        g.FillEllipse(Brushes.Red, minPixelX - 4, minPixelY - 4, 8, 8);
        g.DrawEllipse(Pens.DarkRed, minPixelX - 4, minPixelY - 4, 8, 8);

        // Подписываем точку минимума
        string minText = $"Минимум: ({minX:F3}, {minY:F3})";
        g.DrawString(minText, new Font("Arial", 8, FontStyle.Bold), Brushes.Red, minPixelX + 10, minPixelY - 15);
      }

      // Отображаем готовый bitmap в PictureBox
      graphBox.Image = bmp;
    }
  }
}