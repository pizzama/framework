from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_control(Buff):  # 免控
    def care_group(self):
        return [20]
