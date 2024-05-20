from skills.script import Script


class skill_4000611(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)
    
        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        skill4level = self.get_my_skill_level("400064")
        addhurt=[0,0.1,0.12,0.15,0.2,0.25]
        damage =[0,1.2,1.45,1.7,1.95,2.2]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            if en.hp/en.maxhp>0.5:
               addhurt[skill4level]=0
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*hurt*(1+addhurt[skill4level])*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        for en in enemys:
            # 降低目标防御
            endefens =[0,-0.1,-0.15,-0.2,-0.25,-0.3]
            buf = self.create_buff(self.master, "buf_def", {
                "buff_id": "11004",
                "value":en.defen*endefens[level]
            })

            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf)
