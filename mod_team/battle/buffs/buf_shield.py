from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_shield(Buff):  # 血量上限buff
    def init(self):
        self.sort = 99

    def hurt_execute(self, skill, hurt, total):
        if hurt > 0:  # 对于伤害进处理
            rt = self.value - total
            if hurt > 0:
                self.value = rt
            else:
                return abs(hurt)
        return 0
