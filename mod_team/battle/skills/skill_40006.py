from skills.script import Script


class skill_40006(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()
        
        # 产生伤害
        skldata = self.create_skl_data()
        skill4level = self.get_my_skill_level("400064")
        addhurt=[0,0.1,0.12,0.15,0.2,0.25]
        for en in enemys:
            if en.hp/en.maxhp>0.5:
                addhurt[skill4level]=0
            hurt, htp = self.normal_hurt(en)
            hurt =  hurt*(1+addhurt[skill4level])
            real_hurt = en.hurt(self,hurt)
            self.master.update_maxdander(100)
            maxdander = 100
            en.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)


        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)
        
    def get_skill_type(self):
        return 0
