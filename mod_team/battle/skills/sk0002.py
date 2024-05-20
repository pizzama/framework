from skills.script import Script


class sk0002(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_scope_property(">", "attack", 2)
        if len(enemys) == 0:
            enemys = self.enemys_target_default()

        # 产生伤害
        hurts = []
        for en in enemys:
            hurt = self.master.attack - en.defen
            en.hurt(self, hurt)
            hurts.append(hurt)

        # 随机给一个友军增加治疗buff
        heros = self.heros_random_target(1)
        for hero in heros:
            buf = self.create_buff(self.master, "buf_cure", {"buff_id": "13004"})
            self.skill_buff(hero, buf)

        self.create_skill_log(enemys, hurts)
