﻿using System;
using System.Windows;
using System.Windows.Input;
using Npgsql;

namespace TravelPortal.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Power_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender,
            MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
