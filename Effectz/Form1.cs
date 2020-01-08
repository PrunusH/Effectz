﻿using System;

using Tangine;
using Sulakore.Habbo;
using Sulakore.Modules;
using Sulakore.Protocol;
using System.Linq;

namespace Effectz
{
    [Module("Effectz", "Test all avatar effects")]
    [Author("tonmAr", ResourceName = "GitHub repository", ResourceUrl = "https://github.com/xnumad/Effectz")]
    public partial class GUI : ExtensionForm
    {
        public GUI()
        {
            InitializeComponent();
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            txt_currentid.Text = nmr_startid.Text;
            await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(txt_currentid.Text), 0).ToBytes());
            timer.Start();
            btn_pause.Text = "⏸Pause";
            btn_pause.Visible = true;
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            if (btn_pause.Text == "⏸Pause")
            {
                timer.Stop();
                btn_pause.Text = "▶Resume";
            }
            else if (btn_pause.Text == "▶Resume")
            {
                timer.Start();
                btn_pause.Text = "⏸Pause";
            }
        }

        private async void timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = Convert.ToInt32(nmr_interval.Value);

            if (nmr_stopid.Value > Convert.ToDecimal(txt_currentid.Text))
            {
                txt_currentid.Text = Convert.ToString(Convert.ToInt32(txt_currentid.Text) + 1);
                await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(txt_currentid.Text), 0).ToBytes());
                
            }
            else if (chk_loop.Checked == true)
            {
                txt_currentid.Text = nmr_startid.Text;
                await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(txt_currentid.Text), 0).ToBytes());
            }
            else
            {
                btn_pause.Visible = false;
                timer.Stop();
            }
        }

        private async void btn_applycurrent_Click(object sender, EventArgs e)
        {
            if (!currentIdOk())
                return;

            await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(txt_currentid.Text), 0).ToBytes());
        }

        private bool currentIdOk()
        {
            if (txt_currentid.Text != "" && isDigitsOnly(txt_currentid.Text))
            {
                return true;
            }
            else
            {
                toolTip.Show("Insert the effect ID you want to apply", txt_currentid);
                return false;
            }
        }

        private bool isDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        private void btn_mass_apply_Click(object sender, EventArgs e)
        {
            if (!currentIdOk())
                return;
            
            for (int i = 0; i < (Convert.ToInt32(nmr_entities.Value)); i++)
            {
                Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, i, Convert.ToInt32(txt_currentid.Text), 0).ToBytes());
                System.Threading.Thread.Sleep(Convert.ToInt32(nmr_delay.Value));
            }
        }
    }
}
