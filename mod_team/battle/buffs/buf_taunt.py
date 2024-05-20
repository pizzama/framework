from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_taunt(Buff):  # 嘲讽
    def pre_execute(self):
        return {"silent": 1, "taunt": self.src}
