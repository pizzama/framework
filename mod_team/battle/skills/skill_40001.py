from skills.script import Script


class skill_40001(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        skill4level = self.get_my_skill_level("400014")
        skldata = self.create_skl_data()
        danders=[0,-50,-60,-70,-80,-100]
        addhurt=[0,1,0.6,0.4]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = addhurt[num]*hurt
            real_hurt = en.hurt(self,hurt)
            maxdander = 50
            buff_arr = en.has_buff_id(["1400141", "12004"])
            if len(buff_arr) > 0:
                maxdander =maxdander+danders[skill4level]
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        self.master.update_maxdander(100)
        skldata.set_eng(self.master)

        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)
        
    def get_skill_type(self):
        return 0
