using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SimpleFileIOApp
{
    public class MainForm : Form
    {
        // Элементы интерфейса
        private MenuStrip menuStrip;
        private ToolStrip toolStrip;
        private TextBox textBoxContent;
        private StatusStrip statusStrip;
        private Button btnReadFile;
        private Button btnWriteFile;
        private Button btnClear;

        // Меню
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem openMenu;
        private ToolStripMenuItem saveMenu;
        private ToolStripMenuItem exitMenu;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem aboutMenu;

        // Панель быстрого доступа
        private ToolStripButton btnOpen;
        private ToolStripButton btnSave;
        private ToolStripSeparator toolStripSeparator;
        private ToolStripButton btnAbout;

        // Строка состояния
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel fileInfoLabel;

        public MainForm()
        {
            InitializeComponent();
            UpdateStatus("Готов к работе");
        }

        private void InitializeComponent()
        {
            this.Text = "Работа с текстовыми файлами (Метод 1)";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Создание строки меню
            menuStrip = new MenuStrip();

            // Меню "Файл"
            fileMenu = new ToolStripMenuItem("Файл");
            openMenu = new ToolStripMenuItem("Открыть", null, (s, e) => ReadFile());
            saveMenu = new ToolStripMenuItem("Сохранить", null, (s, e) => WriteFile());
            exitMenu = new ToolStripMenuItem("Выход", null, (s, e) => Application.Exit());

            fileMenu.DropDownItems.Add(openMenu);
            fileMenu.DropDownItems.Add(saveMenu);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitMenu);

            // Меню "Справка"
            helpMenu = new ToolStripMenuItem("Справка");
            aboutMenu = new ToolStripMenuItem("О программе", null, ShowAboutDialog);
            helpMenu.DropDownItems.Add(aboutMenu);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(helpMenu);

            // Создание панели быстрого доступа
            toolStrip = new ToolStrip();

            btnOpen = new ToolStripButton("Открыть", null, (s, e) => ReadFile());
            btnOpen.ToolTipText = "Открыть файл (File.ReadAllText)";

            btnSave = new ToolStripButton("Сохранить", null, (s, e) => WriteFile());
            btnSave.ToolTipText = "Сохранить файл (File.WriteAllText)";

            toolStripSeparator = new ToolStripSeparator();

            btnAbout = new ToolStripButton("О программе", null, ShowAboutDialog);
            btnAbout.ToolTipText = "Информация о программе";

            toolStrip.Items.Add(btnOpen);
            toolStrip.Items.Add(btnSave);
            toolStrip.Items.Add(toolStripSeparator);
            toolStrip.Items.Add(btnAbout);

            // Заголовок
            Label labelTitle = new Label
            {
                Text = "Метод 1: File.ReadAllText / File.WriteAllText",
                Location = new Point(10, 40),
                Size = new Size(400, 25),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.DarkBlue
            };

            // Описание метода
            Label labelDescription = new Label
            {
                Text = "Самый простой способ: чтение и запись всего файла одной операцией",
                Location = new Point(10, 70),
                Size = new Size(500, 20),
                Font = new Font("Arial", 9),
                ForeColor = Color.Gray
            };

            // Кнопка для чтения файла
            btnReadFile = new Button
            {
                Text = "📂 Открыть файл (ReadAllText)",
                Location = new Point(10, 100),
                Size = new Size(200, 40),
                BackColor = Color.LightBlue,
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnReadFile.Click += (s, e) => ReadFile();

            // Кнопка для записи файла
            btnWriteFile = new Button
            {
                Text = "💾 Сохранить файл (WriteAllText)",
                Location = new Point(220, 100),
                Size = new Size(200, 40),
                BackColor = Color.LightGreen,
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnWriteFile.Click += (s, e) => WriteFile();

            // Кнопка очистки
            btnClear = new Button
            {
                Text = "🗑 Очистить",
                Location = new Point(430, 100),
                Size = new Size(100, 40),
                BackColor = Color.LightCoral,
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += (s, e) => ClearTextBox();

            // Создание текстового поля для содержимого файла
            textBoxContent = new TextBox
            {
                Location = new Point(10, 150),
                Size = new Size(760, 260),
                Multiline = true,
                ScrollBars = ScrollBars.Both,
                Font = new Font("Consolas", 11),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Text = "Введите текст здесь или откройте файл для чтения..."
            };

            // Строка состояния
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel("Готов к работе");
            fileInfoLabel = new ToolStripStatusLabel("");

            statusStrip.Items.Add(statusLabel);
            statusStrip.Items.Add(new ToolStripStatusLabel("|"));
            statusStrip.Items.Add(fileInfoLabel);

            // Добавление элементов на форму
            this.Controls.Add(textBoxContent);
            this.Controls.Add(btnReadFile);
            this.Controls.Add(btnWriteFile);
            this.Controls.Add(btnClear);
            this.Controls.Add(labelTitle);
            this.Controls.Add(labelDescription);
            this.Controls.Add(toolStrip);
            this.Controls.Add(menuStrip);
            this.Controls.Add(statusStrip);

            this.MainMenuStrip = menuStrip;
        }

        // Метод чтения файла (File.ReadAllText)
        private void ReadFile()
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.Title = "Выберите текстовый файл для чтения";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // ИСПОЛЬЗУЕМ МЕТОД 1: File.ReadAllText
                        string content = File.ReadAllText(openFileDialog.FileName);
                        textBoxContent.Text = content;

                        UpdateStatus($"Файл прочитан методом File.ReadAllText: {openFileDialog.FileName}");
                        UpdateFileInfo(openFileDialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при чтении файла: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ошибка при чтении файла");
            }
        }

        // Метод записи файла (File.WriteAllText)
        private void WriteFile()
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.Title = "Сохранить текстовый файл";
                    saveFileDialog.OverwritePrompt = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // ИСПОЛЬЗУЕМ МЕТОД 1: File.WriteAllText
                        File.WriteAllText(saveFileDialog.FileName, textBoxContent.Text);

                        UpdateStatus($"Файл сохранен методом File.WriteAllText: {saveFileDialog.FileName}");
                        UpdateFileInfo(saveFileDialog.FileName);

                        MessageBox.Show("Файл успешно сохранен!", "Информация",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                UpdateStatus("Ошибка при сохранении файла");
            }
        }

        // Очистка текстового поля
        private void ClearTextBox()
        {
            textBoxContent.Clear();
            textBoxContent.Text = "Введите текст здесь или откройте файл для чтения...";
            UpdateStatus("Текстовое поле очищено");
            fileInfoLabel.Text = "";
        }

        // Обновление статуса в строке состояния
        private void UpdateStatus(string status)
        {
            statusLabel.Text = status;
        }

        // Обновление информации о файле
        private void UpdateFileInfo(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    string size = FormatFileSize(fileInfo.Length);
                    fileInfoLabel.Text = $"Размер: {size}, Изменен: {fileInfo.LastWriteTime:dd.MM.yyyy HH:mm}";
                }
            }
            catch
            {
                fileInfoLabel.Text = "";
            }
        }

        // Форматирование размера файла
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        // Диалоговое окно "О программе"
        private void ShowAboutDialog(object sender, EventArgs e)
        {
            Form aboutForm = new Form
            {
                Text = "О программе",
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Иконка
            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(64, 64),
                Location = new Point(20, 20),
                BackColor = Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            Bitmap bmp = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightBlue);
                g.DrawString("📄", new Font("Arial", 30), Brushes.DarkBlue, new PointF(15, 10));
            }
            pictureBox.Image = bmp;

            // Информация о программе 
            Label infoLabel = new Label
            {
                Location = new Point(100, 20),
                Size = new Size(260, 120),
                Text = "Разработчик: Хаперскова Юлия Андреевна\n\n" +
                      "Дата создания: 29.05.26\n\n" +
                      "Группа: ИВТ-201\n\n" +
                      "Метод: File.ReadAllText / File.WriteAllText\n\n" +
                      "Версия: 1.0.0",
                Font = new Font("Arial", 10)
            };

            // Кнопка OK
            Button okButton = new Button
            {
                Text = "OK",
                Location = new Point(300, 180),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };

            aboutForm.Controls.Add(pictureBox);
            aboutForm.Controls.Add(infoLabel);
            aboutForm.Controls.Add(okButton);

            aboutForm.ShowDialog(this);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}


