using System;
using Gtk;

public partial class MainWindow: Gtk.Window
{   
    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        // Создаем окно
        SetDefaultSize(250, 150);
        SetPosition(WindowPosition.Center);
        DeleteEvent += delegate { Application.Quit(); };
        
        // Создаем метку
        Label label = new Label();
        label.Text = "Привет, мир!";
        
        // Добавляем метку на окно
        Add(label);
        
        // Показываем все виджеты
        ShowAll();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
    
    public static void Main(string[] args)
    {
        Application.Init();
        MainWindow win = new MainWindow();
        Application.Run();
    }
}
