from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_silent(Buff):  # 沉默
    def pre_execute(self):
        return {"silent": 1}
