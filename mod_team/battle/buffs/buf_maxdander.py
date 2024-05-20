from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_maxdander(Buff):  # 能量变化
    def aft_execute(self, battlelog):
        value = self.target.maxdander + self.value
        self.target.set_maxdander(value)
