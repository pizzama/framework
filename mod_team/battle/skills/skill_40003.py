from skills.script import Script


class skill_40003(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()
        
        # 产生伤害
        skldata = self.create_skl_data()
        skill4level = self.get_my_skill_level("400034")
        damage=[0,-0.05,-0.1,-0.15,-0.2,-0.25]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            real_hurt = en.hurt(self,hurt)
            self.master.update_maxdander(100)
            maxdander = 100
            en.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
            buff_arr = en.has_buff_id(["13001","13002"])
            if len(buff_arr) > 0:
                hp = self.master.attack*damage[skill4level]
                self.master.hurt(self,hp)
                skldata.add_des(self.master)
                skldata.add_hurt(hp)


        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)
        
    def get_skill_type(self):
        return 0
