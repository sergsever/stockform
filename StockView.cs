using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace stockform
{
	public partial class StockView : Form
	{
	private StockViewModel viewModel;
	public StockView(StockViewModel viewModel)
	{
		this.viewModel = viewModel;
		InitializeComponent();
		listBox1.DataSource = viewModel.PricesSource;
		listBox1.DisplayMember = "Name";
	}
	}
}
