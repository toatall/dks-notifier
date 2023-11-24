
namespace DKSNotifier.Formatter
{
    /// <summary>
    /// Приведение данных к html-формату
    /// в виде таблицы
    /// </summary>
    internal class HtmlFormatter: IFormatter
    {      

        /// <inheritdoc/>
        public string GetText(string title, string[] headers, string[][] rows)
        {
            return
                "<h3>" + title + ":</h3>"
                + "<table>"
                + this.GenerateHead(headers)
                + this.GenerateBody(rows)
                + "</table>";
        }

        /// <summary>
        /// Генерация заголовков таблицы
        /// </summary>
        /// <param name="headers">заголовки</param>
        /// <returns>сгенерированный html-текст</returns>
        protected string GenerateHead(string[] headers)
        {
            string result = "<tr>";
            foreach(string header in headers)
            {
                result += "<th>" + header + "</th>";
            }
            return result + "</tr>";           
        }

        /// <summary>
        /// Генерация тела таблицы
        /// </summary>
        /// <param name="rows">данные</param>
        /// <returns>сгенерированный html-текст</returns>
        protected string GenerateBody(string[][] rows)
        {
            string result = "";
            foreach (string[] row in rows)
            {
                result += "<tr>";
                foreach(string item in row)
                {
                    result += "<td>" + item + "</td>";
                }
                result += "</tr>";
            }
            return result;
        }


    }
}
