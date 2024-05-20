from skills.script import Script


class skill_5000151(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,4.5,4.5,4.5,4.5,4.5]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*hurt
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 给敌人加一个降低攻击的buff
            atks = [0,-0.3,-0.3,-0.3,-0.3,-0.3]
            buf = self.create_buff(self.master, "buf_atk", {
                "buff_id":"11002",
                "value": en.attack*atks[level],
            })
            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)


