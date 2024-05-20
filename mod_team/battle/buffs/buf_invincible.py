from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_invincible(Buff):  # 无敌效果 不增加buff 不受伤害
    def hurt_execute(self, skill, hurt, total):
        return 9999999999999

    def care_group(self):
        return [0]
