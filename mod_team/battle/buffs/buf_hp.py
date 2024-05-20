from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_hp(Buff):  # 中毒, 灼烧，流血，恢复
    def aft_execute(self, battlelog):
        if not self.target.is_dead():
            self.target.base_hurt(self.value)
            battlelog.lck_record(self.src.id)
            battlelog.buff_action_record(self, self.src.id, [self.target.id], [self.value])
            battlelog.rls_record(self.src.id)
