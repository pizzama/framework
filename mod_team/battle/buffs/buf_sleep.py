from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_sleep(Buff):  # 眩晕,冰冻,睡眠,麻痹
    def pre_execute(self):
        return {"sleep": 1}
