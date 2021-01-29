using dotnet_rpg.Models;

namespace dotnet_rpg.Models
{
    public class Character
    {
        public int Id { get; set; }        
        public string Name { get; set; } = "frodo";        
        public int HitPoints { get; set; } =10;
        public int Strenghts { get; set; }=10;
        public int Defense { get; set; }=10;
        public int Intelligence { get; set; }=10;
        public RpgClass Class {get;set;}=RpgClass.Knight;
    }
}