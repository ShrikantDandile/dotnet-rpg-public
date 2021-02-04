using System.Collections.Generic;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using dotnet_rpg.Dtos.CharacterSkill;
namespace dotnet_rpg.Dtos.Character
{
    public class GetCharacterDTO
    {
          public int Id { get; set; }        
        public string Name { get; set; } = "frodo";        
        public int HitPoints { get; set; } =10;
        public int Strenghts { get; set; }=10;
        public int Defense { get; set; }=10;
        public int Intelligence { get; set; }=10;
        public RpgClass Class {get;set;}=RpgClass.Knight;
        public GetWeaponDTO Weapon { get; set; }
        public List<GetSkillDTO> Skills { get; set; }
        
        
        
        
    }
}