from skills.script import Script


class skill_4000151(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skill4level = self.get_my_skill_level("400014")
        skldata = self.create_skl_data()
        damage = [0,2.5,2.5,2.5,2.5,2.5]
        danders=[0,-50,-60,-70,-80,-100]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            buff_arr = en.has_buff_id(["1400141", "12004"])
            if len(buff_arr) > 0:
                maxdander = danders[skill4level]
                en.update_maxdander(maxdander)
                skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 随机给一个敌人添加沉默buff
            buf = self.create_buff(self.master, "buf_silent",{
                "buff_id":"1400141",
                "count":3,
            })
            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)

       
