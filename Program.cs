using System;
using System.IO;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        {
            string filePath = "/caminho_do_arquivo/financial_transaction_message.dat"; // atribuir o caminho do arquivo
            //string filePath = "/caminho_do_arquivo/message_with_hex_bcd.dat";
            string unicodeContent = ReadEbcdicFile(filePath);
            Console.WriteLine("Leitura do arquivo EBCDIC:\n" + unicodeContent);
        }

        static string ReadEbcdicFile(string filePath)
        {
            // Ler bytes do arquivo EBCDIC
            byte[] ebcdicBytes = File.ReadAllBytes(filePath);

            // Converter de EBCDIC para Unicode
            string unicodeContent = EbcdicToUnicode(ebcdicBytes);

            // Extrair campos específicos e formatar a saída
            string message = ExtractField(0, 4, unicodeContent);
            string bitmap = ExtractField(4, 64, unicodeContent);
            string processingCode = ExtractField(68, 6, unicodeContent);
            string transactionAmoaunt  = ExtractField(74, 12, unicodeContent);
            string transmissionDateTime = ExtractField(86, 14, unicodeContent);
            string FeeAmount = ExtractField(100, 12, unicodeContent);
            string SystemTraceAuditNumber = ExtractField(112, 6, unicodeContent);
            string LocalTransactionTime = ExtractField(118, 6, unicodeContent);
            string SettlementDate = ExtractField(124, 4, unicodeContent);
            string AcquiringCountryCode = ExtractField(128, 3, unicodeContent);
            string PANCountryCode = ExtractField(131, 3, unicodeContent);
            string PANSequenceNumber = ExtractField(134, 3, unicodeContent);
            string FunctionCode = ExtractField(137, 3, unicodeContent);
            string POSCaptureCode = ExtractField(140, 2, unicodeContent);
            string AcquiringIdentification = ExtractField(142, 1, unicodeContent);
            string Track2 = ExtractField(143, 40, unicodeContent);

            // Formatar a saída conforme desejado
            string textFormated = $"Message: {message}\n" +
                                  $"Bitmap: {bitmap}\n" +
                                  $"03 Processing Code (6): {processingCode}\n" +
                                  $"04 Transaction Amount (12): {transactionAmoaunt}\n" +
                                  $"07 Transmission Date & Time (14): {transmissionDateTime}\n" +
                                  $"08 Fee Amount (12): {FeeAmount}\n" +
                                  $"11 System Trace Audit Number (6): {SystemTraceAuditNumber}\n" +
                                  $"12 Local Transaction Time (6): {LocalTransactionTime}\n" +
                                  $"15 Settlement Date (4): {SettlementDate}\n" +
                                  $"19 Acquiring Country Code (3): {AcquiringCountryCode}\n" +
                                  $"20 PAN Country Code (3): {PANCountryCode}\n" +
                                  $"23 PAN Sequence Number (3): {PANSequenceNumber}\n" +
                                  $"24 Function Code (3): {FunctionCode}\n" +
                                  $"26 POS Capture Code (2): {POSCaptureCode}\n" +
                                  $"32 Acquiring Identification Code (1): {AcquiringIdentification}\n" +
                                  $"35 Track 2 (56): {Track2}";

            return textFormated;
        }

        static string EbcdicToUnicode(byte[] ebcdicBytes)
        {
            Encoding ebcdic = Encoding.GetEncoding("UTF-8"); // EBCDIC para ASCII (IBM037)
            Encoding unicode = Encoding.Unicode;

            // Converter bytes de EBCDIC para Unicode
            byte[] unicodeBytes = Encoding.Convert(ebcdic, unicode, ebcdicBytes);

            // Retornar string Unicode
            return unicode.GetString(unicodeBytes);
        }

        static string ExtractField(int startIndex, int length, string text)
        {
            if (startIndex + length <= text.Length)
                return text.Substring(startIndex, length).Trim();
            else
                return "";
        }
    }
}