from skills.script import Script


class skill_20001(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        level4 = self.get_my_skill_level("200014")
        skldata = self.create_skl_data()
        danders = [0,2,4,6,8,10]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            real_hurt = en.hurt(self,hurt)
            self.master.update_maxdander(100+danders[level4])
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
