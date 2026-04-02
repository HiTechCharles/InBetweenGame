using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Speech.Synthesis.TtsEngine;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace InBetweenGame
{
    public partial class NewGame : Form
    {
        private Form1 parentForm;
        public int RandomChipAmount { get; private set; }
        private int RandomCPU;
        

        public NewGame(Form1 parent)
        {
            InitializeComponent();
            parentForm = parent;

                     //random number between 100 and 1000 for starting chips, divisible by 10
            RandomChipAmount = parent._rng.Next(10, 101) * 10;
            RandomCPU = parent._rng.Next(0, ComputerNameCB.Items.Count + 1);
            ComputerNameCB.SelectedIndex = RandomCPU;

            UpdateChipDisplay();
        }

        private void UpdateChipDisplay()
        {
            HelpLBL.Text = $"Configure New Game\n\n" +
                          $"Starting Chips: {RandomChipAmount}\n\n" +
                          $"Add 2-6 players (Human or Computer) to begin.\n" +
                          $"Type a name and click Add to include players.";
        }

        // Method to get the configured players
            public List<Form1.PlayerInfo> GetConfiguredPlayers()
            {
                var players = new List<Form1.PlayerInfo>();

                // Add human players from HumanLB
                foreach (var item in HumanLB.Items)
                {
                    string playerName = item.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(playerName))
                    {
                        players.Add(new Form1.PlayerInfo
                        {
                            Name = playerName,
                            Type = "Human",
                            Chips = RandomChipAmount,
                            Wins = 0,
                            Losses = 0,
                            WinStreak = 0,
                            WinPercentage = 0.0
                        });
                    }
                }

                // Add computer players from ComputerLB
                foreach (var item in ComputerLB.Items)
                {
                    string playerName = item.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(playerName))
                    {
                        players.Add(new Form1.PlayerInfo
                        {
                            Name = playerName,
                            Type = "Computer",
                            Chips = RandomChipAmount,
                            Wins = 0,
                            Losses = 0,
                            WinStreak = 0,
                            WinPercentage = 0.0
                        });
                    }
                }

                return players;
            }

        // Validate that we have at least 2 players
        private bool ValidatePlayers()
        {
            int playerCount = HumanLB.Items.Count + ComputerLB.Items.Count;

            if (playerCount < 2)
            {
                MessageBox.Show("You need at least 2 players to start a game.",
                    "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (playerCount > 6)
            {
                MessageBox.Show("Maximum of 6 players allowed.",
                    "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidatePlayers())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ComputerLB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void HumanAddBTN_Click(object sender, EventArgs e)
        {
            //Human Add 
            if (HumanNameTB.Text.Length > 0)
            {
                HumanLB.Items.Add(HumanNameTB.Text);
                HumanNameTB.Clear();
                HumanNameTB.Focus();
            }
        }

        private void HumanDelBTN_Click(object sender, EventArgs e)
        {
            //Human Delete
            if (HumanLB.SelectedItem != null)
            {
                HumanLB.Items.Remove    (HumanLB.SelectedItem);
            }
        }

        private void ComputerAddBTN_Click(object sender, EventArgs e)
        {
            //Computer Add
            if ( ComputerNameCB.Text.Length > 0 )
            {
                ComputerLB.Items.Add(ComputerNameCB.Text);
                ComputerNameCB.Items.Remove(ComputerNameCB.Text);
                ComputerNameCB.Focus();
            }
        }

        private void ComputerRemoveBTN_Click(object sender, EventArgs e)
        {
            //Computer Delete
            if (ComputerLB.SelectedItem != null)
            {
                ComputerLB.Items.Remove(ComputerLB.SelectedItem);
            }
        }
    }
}   