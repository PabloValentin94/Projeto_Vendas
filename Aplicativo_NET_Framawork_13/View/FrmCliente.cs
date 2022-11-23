﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data.MySqlClient;

namespace Aplicativo_NET_Framawork_13.View
{

    public partial class FrmCliente : Form
    {

        Model.Cidade cidade;

        Model.Cliente cliente;

        public FrmCliente()
        {

            InitializeComponent();

            desabilitar_elementos();

            carregar_dgv();

            btn_alterar.Enabled = false;

            btn_excluir.Enabled = false;

        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {

            // Definindo permissões do usuário.

            dgv_registros.AllowUserToAddRows = false;

            dgv_registros.AllowUserToDeleteRows = false;

            dgv_registros.AllowUserToOrderColumns = true;

            // Configurando o DataGridView.

            dgv_registros.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgv_registros.ReadOnly = true;

            /*dtpicker_data_nascimento_cliente.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            dtpicker_data_nascimento_cliente.Format = DateTimePickerFormat.Custom;*/

            // Configurando a ComboBox.

            this.cidade = new Model.Cidade();

            cb_cidade.DataSource = this.cidade.Select();

            cb_cidade.DisplayMember = "nome";

            cb_cidade.ValueMember = "id";

            cb_cidade.Text = "";

            carregar_dgv();

            /*DataRowView get_campos = (DataRowView)cb_cidade.SelectedItem;

            txt_unidade_federal.Text = get_campos["uf"].ToString();*/

            txt_unidade_federal.Clear();

            // Ocultando colunas do DataGridView.

            dgv_registros.Columns["foto"].Visible = false;

            dgv_registros.Columns["fk_cidade"].Visible = false;

            // Configurando o DataTimePicker.

            dttmpck_data_nascimento_cliente.MinDate = new DateTime(DateTime.Now.Year - 100, DateTime.Now.Month, DateTime.Now.Day);

            dttmpck_data_nascimento_cliente.MaxDate = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);

            // Fazendo com que a imagem caiba dentro da PictureBox.

            pctbox_foto.SizeMode = PictureBoxSizeMode.Zoom;

        }

        private void btn_fechar_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("O formulário será fechado.", "Aviso!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
               == DialogResult.OK)
            {

                this.Close();

            }

        }

        private void btn_incluir_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txt_nome_cliente.Text) || String.IsNullOrEmpty(txt_email_cliente.Text) ||
                String.IsNullOrEmpty(mtxt_cpf.Text) || String.IsNullOrEmpty(txt_telefone_cliente.Text) ||
                String.IsNullOrEmpty(cb_cidade.Text) || String.IsNullOrEmpty(txt_renda.Text))
            {

                MessageBox.Show("Preencha todos os campos antes de prosseguir.",
                                "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            else
            {

                this.cliente = new Model.Cliente()
                {

                    nome = txt_nome_cliente.Text,

                    email = txt_email_cliente.Text,

                    cpf = funcao_demascarar_cpf(mtxt_cpf.Text),

                    telefone = txt_telefone_cliente.Text,

                    data_nascimento = dttmpck_data_nascimento_cliente.Value,

                    renda = double.Parse(txt_renda.Text),

                    foto = pctbox_foto.ImageLocation,

                    venda = chbox_bloqueio_venda.Checked,

                    fk_cidade = (int)cb_cidade.SelectedValue,

                };

                this.cliente.Incluir();

                limpar_campos();

                carregar_dgv();

            }

        }

        private void btn_alterar_Click(object sender, EventArgs e)
        {



        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txt_codigo_cliente.Text) && String.IsNullOrEmpty(txt_nome_cliente.Text)
               && String.IsNullOrEmpty(txt_email_cliente.Text) && String.IsNullOrEmpty(mtxt_cpf.Text)
               && String.IsNullOrEmpty(txt_telefone_cliente.Text) && String.IsNullOrEmpty(cb_cidade.Text)
               && String.IsNullOrEmpty(txt_unidade_federal.Text) && String.IsNullOrEmpty(txt_renda.Text)
               && btn_pesquisar.Enabled == false)
            {

                MessageBox.Show("Não há nada a ser cancelado.", "Atenção!",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

            else
            {

                limpar_campos();

                desabilitar_elementos();

            }

        }

        private void btn_excluir_Click(object sender, EventArgs e)
        {

            this.cliente = new Model.Cliente()
            {

                id = int.Parse(txt_codigo_cliente.Text)

            };

            if (MessageBox.Show("Realmente deseja apagar o registro " + txt_codigo_cliente.Text + "?", "Atenção!",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                this.cliente.Deletar();

                limpar_campos();

                desabilitar_elementos();

                carregar_dgv();

                MessageBox.Show("Exclusão efetuada com sucesso.", "Atenção!",
                                MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        void limpar_campos()
        {

            txt_codigo_cliente.Clear();

            txt_nome_cliente.Clear();

            txt_email_cliente.Clear();

            mtxt_cpf.Clear();

            txt_telefone_cliente.Clear();

            cb_cidade.Text = "";

            txt_unidade_federal.Clear();

            txt_renda.Clear();

        }

        void carregar_dgv()
        {

            this.cliente = new Model.Cliente();

            this.cliente.Select();

            dgv_registros.DataSource = Database.tabela_de_dados;

        }

        void desabilitar_elementos()
        {

            // Desabilitando os elementos desnecessários:

            btn_alterar.Enabled = false;

            btn_excluir.Enabled = false;

            btn_pesquisar.Enabled = false;

            btn_resetar.Enabled = false;

            // Habilitando os elementos necessários:

            txt_nome_cliente.Enabled = true;

            btn_incluir.Enabled = true;

        }

        private void pctbox_foto_Click(object sender, EventArgs e)
        {

            ofd_foto.InitialDirectory = "C:\\Users\\Evandro\\Downloads";

            ofd_foto.FileName = "";

            ofd_foto.ShowDialog();

            pctbox_foto.ImageLocation = ofd_foto.FileName;

        }

        private void cb_cidade_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataRowView get_campos = (DataRowView)cb_cidade.SelectedItem;

            txt_unidade_federal.Text = get_campos["uf"].ToString();

        }

        string funcao_demascarar_cpf(string cpf)
        {

            string cpf_mascarado = cpf;

            string cpf_desmascarado = "";

            for (int x = 0; x < 14; x++)
            {

                if (cpf_mascarado[x].ToString() != "." && cpf_mascarado[x].ToString() != "-")
                {

                    cpf_desmascarado += cpf_mascarado[x].ToString();

                }
            
            }

            return cpf_desmascarado;

        }

        private void dgv_registros_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (dgv_registros.RowCount > 0)
            {

                if (btn_alterar.Enabled == false && btn_excluir.Enabled == false)
                {

                    string texto = "Opções disponíveis:\n\n" +
                                   "Alterar: caso deseje alterar um registro já existente, preencha " +
                                   "o campo Nome Completo com a nova " +
                                   "informação. O campo Código serve somente para referenciar o registro " +
                                   "que foi clicado duas vezes.\n\n" +
                                   "Deletar: nesse caso, apenas de um duplo clique no registro " +
                                   "que deseja apagar.";

                    MessageBox.Show(texto, "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                // Habilitando os elementos necessários:

                txt_nome_cliente.Enabled = true;

                btn_alterar.Enabled = true;

                btn_excluir.Enabled = true;

                // Desabilitando os elementos desnecessários:

                txt_email_cliente.Enabled = false;

                txt_telefone_cliente.Enabled = false;

                cb_cidade.Enabled = false;

                txt_unidade_federal.Enabled = false;

                dttmpck_data_nascimento_cliente.Enabled = false;

                txt_renda.Enabled = false;

                btn_incluir.Enabled = false;

                // Definido os valores das TextBoxes de pesquisa:

                txt_codigo_cliente.Text = dgv_registros.CurrentRow.Cells[0].Value.ToString();

                txt_nome_cliente.Text = dgv_registros.CurrentRow.Cells[1].Value.ToString();

            }

        }

        private void dgv_registros_Click(object sender, EventArgs e)
        {

            if (btn_pesquisar.Enabled == false)
            {

                MessageBox.Show("Digite no campo Nome Completo e então clique em pesquisar para encontrar um determinado registro. " +
                                "Caso a tabela não esteja mostrando todos os seus registros, clique em resetar antes de fazer a pesquisa.",
                                "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            txt_nome_cliente.Enabled = true;

            btn_pesquisar.Enabled = true;

            btn_resetar.Enabled = true;

        }

        private void btn_pesquisar_Click(object sender, EventArgs e)
        {

            /* Essa condição garante que o usuário só irá fazer outra pesquisa, quando o DataGridView estiver resetado, ou seja, 
             * quando todas as linhas estiverem visiveis. */

            int total_linhas = linhas_totais();

            if (dgv_registros.RowCount < total_linhas)
            {

                MessageBox.Show("Clique no botão resetar antes de fazer outra pesquisa.",
                                "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

            else
            {

                this.cliente = new Model.Cliente()
                {

                    nome = txt_nome_cliente.Text

                };

                this.cliente.SelectByName();

                dgv_registros.DataSource = Database.tabela_de_dados;

                limpar_campos();

                desabilitar_elementos();

            }

        }

        private void btn_resetar_Click(object sender, EventArgs e)
        {

            int total_linhas = linhas_totais();

            if (dgv_registros.RowCount != total_linhas)
            {

                carregar_dgv();

            }

            else
            {

                MessageBox.Show("A tabela não precisa ser resetada. Prossiga com a pesquisa.",
                                "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        int linhas_totais()
        {

            this.cliente = new Model.Cliente();

            this.cliente.Select();

            int qnt_linhas = 0;

            foreach (DataRow rows in Database.tabela_de_dados.Rows)
            {

                qnt_linhas++;

            }

            return qnt_linhas;

        }

    }

}
