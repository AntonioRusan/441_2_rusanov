﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2_UI_Text_Question_Answerer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TabItem> tabItems;
        private int tabCount;
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                // initialize tabItem array
                tabItems = new List<TabItem>();

                // add a tabItem with + in header 
                TabItem tabAdd = new TabItem();
                tabAdd.Header = "+";

                tabItems.Add(tabAdd);

                // bind tab control
                tabDynamic.DataContext = tabItems;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private TabItem AddTabItem()
        {
            int count = tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = string.Format("Tab {0}", tabCount);
            tab.Name = string.Format("tab{0}", tabCount);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;
            tabCount++;

            // add controls to tab item, this case I added just a text box
            StackPanel panel = new StackPanel();
            tab.Content = panel;
            tab.ContentTemplate = tabDynamic.FindResource("TabContent") as DataTemplate;

            // insert tab item right before the last (+) tab item
            tabItems.Insert(count - 1, tab);
            return tab;
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem? tab = tabDynamic.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Header.ToString() == "+")
                {
                    // clear tab control binding
                    tabDynamic.DataContext = null;

                    // add new tab
                    TabItem newTab = this.AddTabItem();

                    // bind tab control
                    tabDynamic.DataContext = tabItems;

                    // select newly added tab item
                    tabDynamic.SelectedItem = newTab;
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = tabDynamic.Items.Cast<TabItem>().Where
                       (i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (tabItems.Count < 2)
                {
                    MessageBox.Show("No tabs to remove!");
                }
                else if (MessageBox.Show(string.Format
                ("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = tabDynamic.SelectedItem as TabItem;

                    // clear tab control binding
                    tabDynamic.DataContext = null;

                    tabItems.Remove(tab);

                    // bind tab control
                    tabDynamic.DataContext = tabItems;

                    // select previously selected tab
                    tabDynamic.SelectedItem = selectedTab;
                }
            }
        }
    }

}
