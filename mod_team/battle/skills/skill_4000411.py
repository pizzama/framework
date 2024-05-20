from skills.script import Script


class skill_4000411(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level # 获得当前技能等级
        skldata = self.create_skl_data()
        damage = [0,1.25,1.5,1.75,2.0,2.25]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 给敌人加一个降低防御的buff效果
            endef = en.defen
            defens = [0,-0.08,-0.1,-0.12,-0.16,-0.2]
            fdef = defens[level] * endef
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id":"11004",
                "value": fdef,
            })

            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)

        
