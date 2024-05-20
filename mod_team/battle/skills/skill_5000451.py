from skills.script import Script


class skill_5000451(Script):
    def execute(self):
        # 筛选目标
        enemys = self.heros_all()
        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.6,0.6,0.6,0.6,0.6]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = 0
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 给队友添加攻击,防御buff
            buf = self.create_buff(self.master, "buf_atk", {
                "buff_id": "1500411",
                "value":en.attack*damage[level],
            })
            self.skill_buff(en, buf)

            buf = self.create_buff(self.master, "buf_def", {
                "buff_id": "11003",
                "value":en.defen*damage[level],
            })
            self.skill_buff(en, buf)


