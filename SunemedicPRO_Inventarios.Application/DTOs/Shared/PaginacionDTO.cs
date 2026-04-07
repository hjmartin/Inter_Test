namespace SunemedicPRO_Inventarios.Server.DTOs.Shared
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int totalRecordsPorPagina = 50;

        public int RecordsPorPagina
        {
            get { return recordsPorPagina; }
            set { recordsPorPagina = value > totalRecordsPorPagina ? totalRecordsPorPagina : value; }
        }
    }
}
