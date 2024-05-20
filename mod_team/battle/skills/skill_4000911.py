from skills.script import Script


class skill_4000911(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.25,1.5,1.75,2.0,2.25]
        maxdanders=[0,10,20,30,40,50]
        skill4level = self.get_my_skill_level("400094")

        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            maxdander = maxdanders[skill4level]
            self.master.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 降低目标出手速度
            speeds =[0,-3,-6,-10,-15,-20]
            buf = self.create_buff(self.master, "buf_speed", {
                "buff_id": "11008",
                "value":en.speed*(speeds[level]/100),
            })

            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)


