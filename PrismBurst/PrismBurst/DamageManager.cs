using PrismBurst.Enum;

namespace PrismBurst
{
    internal class DamageManager
    {
        Random random = new Random();

        public int CalculateDamage(Player player, Monster monster, BattleLog battleLog)
        {
            int damage = player.CriticalDamage();

            switch (player.ColorMagic)
            {
                case MagicType.Fire:

                    damage += player.ComboCount * 5;

                    if (player.IsCritical)
                    {
                        battleLog.SetMessage($"크리티컬! " +
                                             $"파이어 어택! " +
                                             $"{damage} 데미지!");
                    }
                    else
                    {
                        battleLog.SetMessage($"파이어 어택! " +
                                             $"{damage} 데미지!");
                    }
                    break;

                case MagicType.Ice:

                    damage += player.ComboCount * 2;
                    player.Shield += player.ComboCount * 2;

                    if (player.IsCritical)
                    {
                        battleLog.SetMessage($"크리티컬! " +
                                             $"{damage} 데미지! " +
                                             $"{player.ComboCount * 2} " +
                                             $"방어력 증가!");
                    }
                    else
                    {
                        battleLog.SetMessage($"{damage} 데미지! " +
                                             $"{player.ComboCount * 2}" +
                                             $" 방어력 증가!");
                    }
                    break;

                case MagicType.Thunder:

                    damage += player.ComboCount * 3;
                    int chance = random.Next(0, 100);

                    if (chance < 30)
                    {
                        monster.State |= StateType.Stun;

                        if (player.IsCritical)
                        {
                            battleLog.SetMessage($"크리티컬! " +
                                                 $"{damage} 데미지! " +
                                                 $"{monster.Name} 스턴!");
                        }
                        else
                        {
                            battleLog.SetMessage($"{damage} 데미지! " +
                                                 $"{monster.Name} 스턴!");
                        }
                    }
                    else
                    {
                        if (player.IsCritical)
                        {
                            battleLog.SetMessage($"크리티컬! " +
                                                 $"스턴 실패! " +
                                                 $"{damage} 데미지!");
                        }
                        else
                        {
                            battleLog.SetMessage($"스턴 실패! " +
                                                 $"{damage} 데미지!");
                        }
                    }
                    break;

                case MagicType.Poison:

                    damage += player.ComboCount * 2;
                    monster.State |= StateType.Poison;

                    if (player.IsCritical)
                    {
                        battleLog.SetMessage($"크리티컬! " +       
                                             $"{damage} 데미지! " +
                                             $"{monster.Name} 중독!");
                    }
                    else
                    {
                        battleLog.SetMessage($"{damage} 데미지! " +
                                             $"{monster.Name} 중독!");
                    }
                    break;
            }
            return damage;
        }

        public int CalculateMonsterDamage(Monster monster, Player player, BattleLog battleLog)
        {
            int damage = monster.CriticalDamage();

            switch (monster.DangerState)
            {
                case DangerLevel.Warning:

                    battleLog.SetMessage("위험 감지!");

                    break;

                case DangerLevel.Critical:

                    battleLog.SetMessage("흉폭 공격!");
                    damage += 5;

                    break;

                case DangerLevel.Berserk:

                    battleLog.SetMessage("광폭화 공격!");
                    damage += 10;

                    break;
            }

            if (monster.IsCritical)
            {
                battleLog.SetMessage($"크리티컬! {battleLog.GetMessage()}");
            }            
            return damage;
        }
    }
}
