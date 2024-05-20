from game.core.mods.mod_team.battle.buffs.buff import Buff


class buf_shield_bomb(Buff):  # 护盾爆炸
    def init(self):
        self.sort = 99
        self.percent = 0.3
        self.left = self.value

    def hurt_execute(self, skill, hurt, total):
        if hurt > 0:  # 对于伤害进处理
            rt = self.left - total
            if hurt > 0:
                self.left = rt
            else:
                # 护盾爆炸对其它人造成伤害
                targets = skill.atks
                ht = self.value * self.percent
                for tar in targets:
                    tar.base_hurt(ht)
                return abs(hurt)
        return 0
