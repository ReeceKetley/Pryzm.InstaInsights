using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InstaInsightsV2.Domain.Models;
using InstaInsightsV2.Helpers;
using InstaInsightsV2.Infrastructure.Instagram;
using InstaInsightsV2.Properties;

namespace InstaInsightsV2.Domain.Campaign
{
    public class Campaign
    {
        public bool GetAll { get; }
        private readonly string _user;
        private readonly DataGridView _grid;
        public Campaign(string user, DataGridView grid, bool getAll)
        {
            GetAll = getAll;
            _user = user;
            _grid = grid;
        }

        public void Update()
        {
            Main();
        }

        private void Main()
        {
            var bot = new InstagramBot(_user, GetAll);
            bot.GetPosts();
            UpdateGrid(_grid, bot);
        }


        private void UpdateGrid(DataGridView grid, InstagramBot bot)
        {
            grid.Invoke((MethodInvoker)delegate
            {
                grid.Enabled = true;
                grid.DataSource = bot.Posts;
                grid.Update();
                grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            });
        }

    }
}
