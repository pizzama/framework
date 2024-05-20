from skills.script import Script


class skill_1000002(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        skldata = self.create_skl_data()
        addhurt=[0,1,0.6,0.4]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = addhurt[num]*hurt
            real_hurt = en.hurt(self,hurt)
            maxdander = 50
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
