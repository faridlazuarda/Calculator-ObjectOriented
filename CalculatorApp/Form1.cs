﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        Double value = 0;
        String operation = "";
        bool operation_pressed = false;
        public Form1()
        {
            InitializeComponent();
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button_Click(object sender, EventArgs e)
        {
            if((result.Text == "0")||(operation_pressed))
            {
                result.Clear();
            }
            operation_pressed = false;
            Button b = (Button)sender;
            if(result.Text == ".")
            {
                if(!result.Text.Contains("."))
                {
                    result.Text = result.Text + b.Text;
                }
            }
            result.Text = result.Text + b.Text;
        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void operator_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            operation = b.Text;
            value = Double.Parse(result.Text);
            operation_pressed = true;
            equation.Text = value + " " +operation;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            equation.Text = "";
            switch (operation)
            {
                case "+":
                    result.Text = (value + Double.Parse(result.Text)).ToString();
                    break;
                case "-":
                    result.Text = (value - Double.Parse(result.Text)).ToString();
                    break;
                case "*":
                    result.Text = (value * Double.Parse(result.Text)).ToString();
                    break;
                case "/":
                    result.Text = (value / Double.Parse(result.Text)).ToString();
                    break;
                default:
                    break;
            }//end switch
        }

        private void button15_Click(object sender, EventArgs e)
        {
            result.Clear();
            value = 0;
            result.Text = "0";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
