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
        public override bool IsRemoteModule => true;

        public GUI()
        {
            InitializeComponent();
            nmr_interval_ValueChanged(null, null);
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            nmr_currentId.Value = nmr_startid.Value;
            await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(nmr_currentId.Value), 0).ToBytes());
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
            if (nmr_currentId.Value >= nmr_stopid.Value) //End reached
            {
                if (chk_loop.Checked)
                    nmr_currentId.Value = nmr_startid.Value;
                else
                {
                    btn_pause.Visible = false;
                    timer.Stop();
                    return;
                }
            }
            else
                nmr_currentId.Value++;

            await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(nmr_currentId.Value), 0).ToBytes());
        }

        private async void btn_applycurrent_Click(object sender, EventArgs e)
        {
            await Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, Convert.ToInt32(nmr_index.Value), Convert.ToInt32(nmr_currentId.Value), 0).ToBytes());
        }

        private void btn_mass_apply_Click(object sender, EventArgs e)
        {            
            for (int i = 0; i < (Convert.ToInt32(nmr_entities.Value)); i++)
            {
                Connection.SendToClientAsync(new HMessage(In.RoomUserEffect, i, Convert.ToInt32(nmr_currentId.Value), 0).ToBytes());
                System.Threading.Thread.Sleep(Convert.ToInt32(nmr_delay.Value));
            }
        }

        private void nmr_interval_ValueChanged(object sender, EventArgs e)
        {
            timer.Interval = (int)nmr_interval.Value;
        }
    }
}
