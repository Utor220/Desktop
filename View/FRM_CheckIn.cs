﻿using Desktop.Controller;
using Desktop.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Desktop.View
{
    public partial class FRM_CheckIn : Form
    {
        CheckIn CheckIn;
        Mensagem Mensagem;
        CTR_CheckIn CTR_CheckIn;
        public FRM_CheckIn()
        {
            InitializeComponent();
            CheckIn = new CheckIn();
            Mensagem = new Mensagem();
            CTR_CheckIn = new CTR_CheckIn();
            CheckIn.HospedadosList = new List<string>();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void FRM_CheckIn_Load(object sender, EventArgs e)
        {
            dateTimeFinal.MinDate = dateTimeInicio.Value.AddDays(1);
            dateTimeInicio.MinDate = DateTime.Now;
        }

        private void lviewQuartos_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected) e.Item.Selected = false;
        }

        private void btnAddMaisHosp_Click(object sender, EventArgs e)
        {
            if (!txbNome.Text.Equals(string.Empty))
            {
                if (!CheckIn.HospedadosList.Contains(txbNome.Text))
                {
                    if (CheckIn.HospedadosList.Count < 4 & CheckIn.IdQuarto.Equals("Quarto Quádruplo"))
                        CheckIn.HospedadosList.Add(txbNome.Text);
                    else if (CheckIn.HospedadosList.Count < 3 & CheckIn.IdQuarto.Equals("Quarto Triplo"))
                        CheckIn.HospedadosList.Add(txbNome.Text);
                    else if (CheckIn.HospedadosList.Count < 2 & CheckIn.IdQuarto.Equals("Quarto Duplo"))
                        CheckIn.HospedadosList.Add(txbNome.Text);
                    else if (CheckIn.HospedadosList.Count < 1 & CheckIn.IdQuarto.Equals("Quarto Individual"))
                        CheckIn.HospedadosList.Add(txbNome.Text);
                    else
                        MessageBox.Show("Voce atingiu o número máximo de pessoas para este quarto");
                }
                else
                {
                    MessageBox.Show("Este hóspede já foi adicionado!");
                }

                lblPeriodo.Visible = true;
                lblAte.Visible = true;
                lblDe.Visible = true;
                dateTimeFinal.Visible = true;
                dateTimeInicio.Visible = true;
                btnConfirmarData.Visible = true;
            }


            if (lviewSubTotal.Items[0].SubItems[1].Text.Equals(" "))
            {
                if (!txbNome.Text.Equals(string.Empty))
                {
                    lviewSubTotal.Items[0].SubItems[1].Text = string.Join(",", CheckIn.HospedadosList);
                    txbNome.Text = string.Empty;
                    maskTxbRg.Text = string.Empty;
                }
                else
                    MessageBox.Show("Por favor digite o documento de ID do cliente!");
            }
            else 
            {
                if (!txbNome.Text.Equals(string.Empty))
                {
                    lviewSubTotal.Items[0].SubItems[1].Text = string.Join(", ", CheckIn.HospedadosList);
                    txbNome.Text = string.Empty;
                    maskTxbRg.Text = string.Empty;
                }
                else
                    MessageBox.Show("Por favor digite o documento de ID do cliente!");
            }
        }

        private void lviewSubTotal_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected) e.Item.Selected = false;
        }

        private void btnEscolherQuartos_Click(object sender, EventArgs e)
        {
            lviewSubTotal.Items.Add(CheckIn.NumeroQuarto);
            lviewSubTotal.Items[0].SubItems.Add(" ");
            lviewSubTotal.Items[0].SubItems.Add(" ");
            lviewSubTotal.Items[0].SubItems.Add(" ");
            HabilitarDepoisQuartos();
            DesabilitarEsquerda();
        }

        private void radBtnTriplo_CheckedChanged(object sender, EventArgs e)
        {
            CheckIn.IdQuarto = "Quarto Triplo";
            HabilitarControles();
            CheckIn.Valor = 270M;
        }

        private void radBtnQuadruplo_CheckedChanged(object sender, EventArgs e)
        {
            CheckIn.IdQuarto = "Quarto Quádruplo";
            HabilitarControles();
            CheckIn.Valor = 360M;
        }

        private void radBtnDuplo_CheckedChanged(object sender, EventArgs e)
        {
            CheckIn.IdQuarto = "Quarto Duplo";
            HabilitarControles();
            CheckIn.Valor = 180M;
        }

        private void radBtnIndividual_CheckedChanged(object sender, EventArgs e)
        {
            CheckIn.IdQuarto = "Quarto Individual";
            HabilitarControles();
            CheckIn.Valor = 100M;
        }

        private void btnCarregarLista_Click(object sender, EventArgs e)
        {
            dgvMapaQuartos.DataSource = null;
            dgvMapaQuartos.Columns.Clear();
            dgvMapaQuartos.Rows.Clear();
            dgvMapaQuartos.Refresh();

            CTR_CheckIn.CarregarLista(CheckIn);

            dgvMapaQuartos.DataSource = CheckIn.Lista;
        }

        private void cBoxSelectQuarto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPesquisar_Click_1(object sender, EventArgs e)
        {

                CheckIn.IdPesquisa = maskTxbRg.Text;

                Mensagem = CTR_CheckIn.PesquisarHospede(CheckIn);

                if (Mensagem.VerificaReturnFuncao.Equals(true))
                {
                    maskTxbRg.Text = CheckIn.IdPesquisa;
                    txbNome.Text = CheckIn.NomePesquisa;
                }
                else
                {
                    MessageBox.Show(Mensagem.TMensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            
        }

        private void dgvMapaQuartos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            CheckIn.NumeroQuarto = Convert.ToString(dgvMapaQuartos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            btnEscolherQuartos.Enabled = true;
            btnEscolherQuartos.Visible = true;
        }

        private void maskTxbRg_Click(object sender, EventArgs e)
        {
            maskTxbRg.SelectionStart = 0;
        }

        private void btnFinalizarCheckIn_Click(object sender, EventArgs e)
        {
            CheckIn.NumeroQuarto = lviewSubTotal.Items[0].Text;
            CheckIn.NomePesquisa = lviewSubTotal.Items[0].SubItems[1].Text;

            Mensagem = CTR_CheckIn.FinalizarCheckIn(CheckIn);

            MessageBox.Show(Mensagem.TMensagem, " ", MessageBoxButtons.OK);

            DialogResult = DialogResult.OK;

        }

        private void HabilitarControles()
        {
            //deixando-os visíveis
            btnCarregarLista.Visible = true;
            dgvMapaQuartos.Visible = true;
        }

        private void HabilitarDepoisQuartos()
        {
            //deixando-os visíveis
            lblMaisHospedes.Visible = true;
            lblRG.Visible = true;
            maskTxbRg.Visible = true;
            txbNome.Visible = true;
            btnAddMaisHosp.Visible = true;
            btnPesquisar.Visible = true;
            lviewSubTotal.Visible = true;
            lblNome.Visible = true;
        }

        private void DesabilitarEsquerda()
        {
            //deixando-os invisíveis
            btnCarregarLista.Visible = false;
            dgvMapaQuartos.Visible = false;
            lviewQuartos.Visible = false;
            radBtnTriplo.Visible = false;
            radBtnQuadruplo.Visible = false;
            radBtnDuplo.Visible = false;
            radBtnIndividual.Visible = false;
            btnEscolherQuartos.Visible = false;
        }

        private void btnConfirmarData_Click(object sender, EventArgs e)
        {
            CheckIn.Chegada = dateTimeInicio.Value;
            CheckIn.Saida = dateTimeFinal.Value;

            CheckIn.PeriodoTotal = Math.Ceiling((CheckIn.Saida - CheckIn.Chegada).TotalDays);
                        
            CheckIn.ValorFinal = CheckIn.Valor * Convert.ToDecimal(CheckIn.PeriodoTotal);

            lviewSubTotal.Items[0].SubItems[2].Text = Convert.ToString(CheckIn.PeriodoTotal + " Noites"); //Fazer uma verificação para quando for apenas uma noite e para quando for 2 ou mais.

            lviewSubTotal.Items[0].SubItems[3].Text = "R$ " + Convert.ToString(CheckIn.ValorFinal);

            btnFinalizarCheckIn.Visible = true;
        }

        private void txbNome_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimeInicio_ValueChanged(object sender, EventArgs e)
        {
            dateTimeFinal.MinDate = dateTimeInicio.Value.AddDays(1);
        }
    }
}
