using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace codeASM2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text;
                string type = cboType.SelectedItem.ToString();
                int lasReading = (int)numLastReading.Value;
                int currentReading = (int)numCurrentReading.Value;
                int peopleCount = (type == "Household") ? (int)numPeople.Value : 1;
                if (currentReading < lasReading)
                {
                    MessageBox.Show("Error", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // tính lượng nước tiêu thụ trong tháng
                int consunption = currentReading - lasReading;
                // gọi hàm 
                (double totalBill, double envFee, double vat, double finalTotal, double unitPrice) = CalculateDetailedBill(type, consunption, peopleCount);
                // hiển thị bill
                lblInvoiceName.Text = $"Customer: {name}";
                lblInvoiceType.Text = $"Customer Type: {type}";
                lblInvoiceLastReading.Text = $"gia nuoc thang truoc: {lasReading} m3";
                lblInvoiceCurrenReading.Text = $"gia nuoc thang nay: {currentReading} m3";
                lblInvoiceConsumption.Text = $"water Consumption: {consunption}  m3";
                lblInvoicePrice.Text = $"gia nuoc cao nhat  : {unitPrice:N0} VND/m3";
                lblInvoiceTotal.Text = $"water cost: {totalBill:N0} VND/m3";
                lblInvoiceEnvFee.Text = $"Environmental Protection Fee (10%): {envFee:N0} VND/m3";
                lblInvoiceVAT.Text = $"VAT: {vat:N0} VND/m3";
                lblInvoiceFinalTotal.Text = $"Total Payment Amount: {finalTotal:N0} VND/m3";
                // Hiển thị thông báo tính xong và đổi màu giao diện
                MessageBox.Show("Calculation Completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //SaveInvoice(name, type, lasReading, currentReading, peopleCount, consunption, unitPrice, totalBill, envFee, vat, finalTotal);

            }
            catch (Exception ex)
            {
                // Nếu có lỗi thì hiển thị thông báo lỗi
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private (double totalBill, double envFee, double vat, double finalTotal, double unitPrice) CalculateDetailedBill(string type, int consumption, int peopleCount)
        {
            //tổng tiền chưa phí và thuế
            double total = 0;
            double unitPrice = 0;
            // tính hộ gia đình
            if (type == "Household")
            {
                double perPersonUsage = (double)consumption / peopleCount;
                int remainingConsumption = consumption;
                if (perPersonUsage > 30) unitPrice = 15929;
                else if (perPersonUsage < 20) unitPrice = 8699;
                else if (perPersonUsage < 10) unitPrice = 7052;
                else unitPrice = 5973;
                //tính > 30
                if (perPersonUsage > 30)
                {
                    int tierConsumption = remainingConsumption - (30 * peopleCount);
                    total += tierConsumption = 15929;
                    remainingConsumption -= tierConsumption;
                }
                //tính > 20
                if (perPersonUsage > 20)
                {
                    int tierConsumption = remainingConsumption - (20 * peopleCount);
                    total += tierConsumption * 8699;
                    remainingConsumption -= tierConsumption;
                }
                //tính > 10
                if (perPersonUsage > 10)
                {
                    int tierConsumption = remainingConsumption - (10 * peopleCount);
                    total += tierConsumption * 7052;
                    remainingConsumption -= tierConsumption;

                }
                //tính > 10
                total += remainingConsumption * 5973;
            }
            else if (type == "Administrative Agency")
            {
                total = consumption * 9955;
                unitPrice = 9955;
            }
            else if (type == "Production Unit")
            {
                total = consumption * 11615;
                unitPrice = 11615;
            }
            else if (type == "Business Service")
            {
                total = consumption * 22068;
                unitPrice = 22068;
            }
            // tính phí envFee
            double envFee = total * 0.1;
            // tính VAT
            double VAT = (total + envFee) * 0.1;
            // tổng bill
            double finalTotal = total + envFee + VAT;
            return (total, envFee, VAT, finalTotal, unitPrice);
        }

    }
}
      
  
