from skills.script import Script


class skill_40009(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        skldata = self.create_skl_data()
        selfdanders=[0,10,20,30,40,50,100]
        selfdander = 0
        addhurt=[0,1,0.6,0.4]
        if self.get_my_skill_level("400095")==1:
            skill4level=6
        else:
            skill4level = self.get_my_skill_level("400094")
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = addhurt[num]*hurt
            real_hurt = en.hurt(self,hurt)
            selfdander= selfdander+selfdanders[skill4level]
            maxdander = 50
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        self.master.update_maxdander(100+selfdander)
        skldata.set_eng(self.master)

        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)

    def get_skill_type(self):
        return 0
