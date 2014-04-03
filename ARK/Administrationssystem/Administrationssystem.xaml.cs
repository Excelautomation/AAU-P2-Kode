﻿using ARK.Administrationssystem;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ARK.Administrationssystem
{
    /// <summary>
    /// Interaction logic for Administrationssystem.xaml
    /// </summary>
    public partial class Administrationssystem : Window
    {
        public Administrationssystem()
        {
            InitializeComponent();

            this.DataContext = new ARK.ViewModel.AdministrationssystemViewModel();
        }
    }
}