from skills.script import Script


class skill_30003(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()
        
        # 产生伤害
        skldata = self.create_skl_data()
        skill4level = self.get_my_skill_level("300034")
        crits = [0, 0.3, 0.35, 0.4, 0.45, 0.5]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            if htp == 1:
                crtrate = crits[skill4level]
                hurt = hurt * (1 + crtrate/1.5)
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
