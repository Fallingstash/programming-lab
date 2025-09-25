using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace labs_programming {
  public partial class MainMenu : Form {
    public MainMenu() {
      InitializeComponent();
      CreateInterface();
    }

    private void CreateInterface() {
      this.Size = new Size(400, 500);
      this.Text = "Главное окно приложения";
      this.StartPosition = FormStartPosition.CenterScreen;

      Label titleLabel = new Label();
      titleLabel.Text = "Мое приложение\nВыберите окно";
      titleLabel.Size = new Size(380, 80);
      titleLabel.Location = new Point(10, 10);
      titleLabel.TextAlign = ContentAlignment.MiddleCenter;
      titleLabel.Font = new Font("Arial", 14, FontStyle.Bold);
      titleLabel.ForeColor = Color.DarkBlue;
      this.Controls.Add(titleLabel);

      Panel buttonsPanel = new Panel();
      buttonsPanel.Size = new Size(380, 380);
      buttonsPanel.Location = new Point(10, 100);
      buttonsPanel.BackColor = Color.White;

      // Создаем 9 кнопок в виде сетки 3x3
      int buttonWidth = 120;
      int buttonHeight = 120;
      int betweenBtns = 5;

      for (int row = 0; row < 3; row++) {
        for (int col = 0; col < 3; col++) {
          int buttonNumber = row * 3 + col + 1;

          Button btn = new Button();
          btn.Text = $"Задание {buttonNumber}";
          btn.Size = new Size(buttonWidth, buttonHeight);
          btn.Location = new Point(col * (buttonWidth + betweenBtns),
                                 row * (buttonHeight + betweenBtns));
          btn.Tag = buttonNumber;
          btn.Font = new Font("Arial", 10, FontStyle.Regular);
          btn.BackColor = Color.LightBlue;
          btn.Click += Btn_Click;

          buttonsPanel.Controls.Add(btn);
        }
      }
      this.Controls.Add(buttonsPanel);
    }

    private void Btn_Click(object sender, EventArgs e) {
      int windowNumber = (int)((Button)sender).Tag;

      switch (windowNumber) {
        case 1:
          new Task1().Show();
          break;
        case 2:
          break;
        case 3:
          break;
        case 4:
          break;
        case 5:
          break;
        case 6:
          break;
        case 7:
          break;
        case 8:
          break;
        case 9:
          break;
      }
    }
  }
}
