from skills.script import Script


class skill_3000211(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.1,1.3,1.5,1.7,1.9]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]
            hurt=hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 给敌人加一个降低防御的buff
            defen=[0,-0.05,-0.07,-0.09,-0.12,-0.15]
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id":"11004",
                "value": en.defen*defen[level],
                "count":3,
            })

            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)