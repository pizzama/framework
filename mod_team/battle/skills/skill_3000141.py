from skills.script import Script


class skill_3000141(Script):
    def execute(self):
        #筛选目标
        en=self.master

        # 添加buff
        level = self.skill.level
        skldata = self.create_skl_data()
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        skldata.add_des(en)

        
        # 给自己加一个减伤的buff效果
        hurts=[0,0.02,0.04,0.07,0.10,0.15]
        buf = self.create_buff(self.master, "buf_hurt", {
            "buff_id":"11018",
            "value": -hurts[level],
        })

        self.skill_buff(en, buf)
        
    def get_skill_type(self):
        return 4   
            
        
