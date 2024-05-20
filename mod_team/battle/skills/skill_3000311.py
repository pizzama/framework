from skills.script import Script


class skill_3000311(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0, 1.3, 1.5, 1.7, 1.9, 2.1]
        crits = [0, 0.3, 0.35, 0.4, 0.45, 0.5]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            skilllevel = self.get_my_skill_level("300034")
            if htp == 1:
                crtrate = crits[skilllevel]
                hurt = hurt * (1 + crtrate/1.5)
            hurt = damage[level] * hurt
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 给敌人加一个降低韧性的buff
            endefcrit = [0, -20, -40, -70, -100, -150]
            buf = self.create_buff(self.master, "buf_defcrit", {
                "buff_id": "11016",
                "value": endefcrit[level],
            })
            self.skill_buff(en, buf)


 
