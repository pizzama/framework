from skills.script import Script


class skill_3000611(Script):
    def execute(self):
        # 筛选目标
        enemys = self.heros_all()

        # 产生伤害
        skldata = self.create_skl_data()
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = -hurt
            en.hurt(self, hurt)
            maxdander = 500
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(hurt)
            skldata.add_htp(htp)

            # 给友方添加防御buff
            buf = self.create_buff(self.master, "buf_def", {
                "def": 100,
            })
            self.skill_buff(en, buf)

        # 创建技能日志
        self.create_skill_log(skldata)
