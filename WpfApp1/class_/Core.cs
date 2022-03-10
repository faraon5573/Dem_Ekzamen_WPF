using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.class_
{
    class Core
    {
        public static Entities DB = new Entities();

        public static List<Material> materialNews { get; set; }
        public static List<Material> _MaterialLis { get; set; }
    }
    class GetItems
    {
        public static List<string> ListFiltr = new List<string> { "Все типы", "Гранулы", "Рулон", "Нарезка", "Пресс" };
    }
}
