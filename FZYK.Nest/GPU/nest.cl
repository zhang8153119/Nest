#define RE 0.00001f

typedef struct 
{ 
	float X;
	float Y; 
} MyPoint; 

/* X扫描线
*/
void SetGridValueX(__global MyPoint* plist,int PCOUNT,int j ,float y,int WI,int T,__global int* result)
{
	MyPoint parray[100];
	int c = 0;
	MyPoint line[100];
	int lc = 0;
	
	for(int m = 0;m<PCOUNT;m++)
	{
		int mnext = (m+1)%PCOUNT;
		
		MyPoint p1;
		MyPoint p2;
		int index1;
		int index2;
		int nodecount = 0;
		bool ifend = false;
		//重合
		if(fabs(plist[m].Y-y) < RE && fabs(plist[mnext].Y-y)<RE)
		{
			p1 = plist[m];
			p2 = plist[mnext];
			index1 = m;
			index2 = mnext;
			nodecount =2;
			ifend = true;
		}
		//平行不重合
		else if(fabs(plist[m].Y-plist[mnext].Y)<RE)
		{
			nodecount = 0;
		}
		//不相交
		else if((y-plist[m].Y<-RE &&y-plist[mnext].Y<-RE)
			|| (y-plist[m].Y>RE &&y-plist[mnext].Y>RE))
		{
			nodecount = 0;
		}
		//过起点
		else if (fabs(plist[m].Y -y) < RE)
		{
			p1.X = plist[m].X;
			p1.Y = y;
			index1 = m;
			nodecount =1;
			ifend = true;
		}
		//过终点
		else if (fabs(plist[mnext].Y -y) < RE)
		{
			p1.X =plist[mnext].X;
			p1.Y = y;
			index1 = mnext;
			nodecount =1;
			ifend = true;
		}
		//垂直
		else if(fabs(plist[m].X - plist[mnext].X) < RE)
		{
			p1.X = plist[m].X;
			p1.Y = y;
			nodecount =1;
			ifend = false;
		}
		//普通相交
		else
		{
			float xielv = (plist[mnext].Y - plist[m].Y) / (plist[mnext].X - plist[m].X);
            float thisx = (y -plist[m] .Y) / xielv + plist[m] .X;
			p1.X = thisx;
			p1.Y = y;
			nodecount = 1;
			ifend = false;
		}
		//非端点
		if(!ifend)
		{
			if(nodecount >= 1)
			{			
				int insertindex = c;
				bool ifinsert = true;
				for(int n = 0;n<c;n++)
				{
					if(fabs(parray[n].X-p1.X)<RE && fabs(parray[n].Y-p1.Y)<RE)
					{
						ifinsert = false;
						break;
					}
					if( p1.X < parray[n].X)
					{
						insertindex = n;
						break;
					}
				}
				
				if(ifinsert)
				{
					for(int k = c-1;k>=insertindex;k--)
					{
						parray[k+1] = parray[k];
					}
					parray[insertindex] = p1;
					c++;
				}
			}//end if nodecount >=1
			if(nodecount == 2)
			{			
				int insertindex = c;
				bool ifinsert = true;
				for(int n = 0;n<c;n++)
				{
					if(fabs(parray[n].X-p2.X)<RE && fabs(parray[n].Y-p2.Y)<RE)
					{
						ifinsert = false;
						break;
					}
					if( p2.X < parray[n].X)
					{
						insertindex = n;
						break;
					}
				}
				
				if(ifinsert)
				{
					for(int k = c-1;k>=insertindex;k--)
					{
						parray[k+1] = parray[k];
					}
					parray[insertindex] = p2;
					c++;
				}
			}//end if nodecount == 2
		}//end if !ifend
		//端点
		else
		{
			if(nodecount >=1)
			{
				int lastindex = (index1-1+PCOUNT)%PCOUNT;
				int nextindex = (index1+1)%PCOUNT;
				int nextindex2 = (index1+1)%PCOUNT;
				if(plist[index1].Y == plist[lastindex].Y)
				{
					bool find = false;
					for(int f = 0;f<lc;f++)
					{
						if(line[f].X == plist[index1].X && line[f].Y == plist[index1].Y)
						{
							find = true;
							break;
						}
						if(find)
						{
							int insertindex = lc;
							bool ifinsert = true;
							for(int n = 0;n<lc;n++)
							{
								if(fabs(line[n].X-plist[index1].X)<RE && fabs(line[n].Y-plist[index1].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index1].X < line[n].X)
								{
									insertindex = n;
									break;
								}
							}//end for int n = 0;n<lc;n++
							if(ifinsert)
							{
								for(int k = lc-1;k>=insertindex;k--)
								{
									line[k+1] = line[k];
								}
								line[insertindex] = plist[index1];
								lc++;
							}
						}//end if find
					}//end for int f = 0;f<lc;f++
				}//end if plist[index1].Y == plist[lastindex].Y
				else if(plist[index1].Y == plist[nextindex].Y)
				{
					int insertindex = lc;
					bool ifinsert = true;
					for(int n = 0;n<lc;n++)
					{
						if(fabs(line[n].X-plist[index1].X)<RE && fabs(line[n].Y-plist[index1].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if( plist[index1].X < line[n].X)
						{
							insertindex = n;
							break;
						}
					}//end for int n = 0;n<lc;n++
					if(ifinsert)
					{
						for(int k = lc-1;k>=insertindex;k--)
						{
							line[k+1] = line[k];
						}
						line[insertindex] = plist[index1];
						lc++;
					}
					
					if(!((plist[lastindex].Y > plist[index1].Y && plist[nextindex2].Y > plist[nextindex].Y)
						|| (plist[lastindex].Y < plist[index1].Y && plist[nextindex2].Y < plist[nextindex].Y)))
					{
							int insertindex = c;
							bool ifinsert = true;
							for(int n = 0;n<c;n++)
							{
								if(fabs(parray[n].X-plist[index1].X)<RE && fabs(parray[n].Y-plist[index1].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index1].X < parray[n].X)
								{
									insertindex = n;
									break;
								}
							}
				
							if(ifinsert)
							{
								for(int k = c-1;k>=insertindex;k--)
								{
									parray[k+1] = parray[k];
								}
								parray[insertindex] = plist[index1];
								c++;
							}
					}//end if !(...)
				}//end else if plist[index1].Y == plist[nextindex].Y
				else if ((plist[lastindex] .Y > plist[index1] .Y && plist[nextindex].Y > plist[index1].Y)
					|| (plist[lastindex] .Y < plist[index1] .Y && plist[nextindex].Y < plist[index1].Y) )
				{
					int insertindex = c;
					bool ifinsert = true;
					for(int n = 0;n<c;n++)
					{
						if(fabs(parray[n].X-plist[index1].X)<RE && fabs(parray[n].Y-plist[index1].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if(plist[index1].X < parray[n].X)
						{
							insertindex = n;
							break;
						}
					}
				
					if(ifinsert)
					{
						for(int k = c-1;k>=insertindex;k--)
						{
							parray[k+2] = parray[k];
						}
						parray[insertindex+1] = plist[index1];
						parray[insertindex] = plist[index1];
						c+=2;
					}
				}//end else if (...)
			}//end if nodecount >=1
			if (nodecount ==2)
			{
				int lastindex = (index2-1+PCOUNT)%PCOUNT;
				int nextindex = (index2+1)%PCOUNT;
				int nextindex2 = (index2+1)%PCOUNT;
				if(plist[index2].Y == plist[lastindex].Y)
				{
					bool find = false;
					for(int f = 0;f<lc;f++)
					{
						if(line[f].X == plist[index2].X && line[f].Y == plist[index2].Y)
						{
							find = true;
							break;
						}
						if(find)
						{
							int insertindex = lc;
							bool ifinsert = true;
							for(int n = 0;n<lc;n++)
							{
								if(fabs(line[n].X-plist[index2].X)<RE && fabs(line[n].Y-plist[index2].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index2].X < line[n].X)
								{
									insertindex = n;
									break;
								}
							}//end for int n = 0;n<lc;n++
							if(ifinsert)
							{
								for(int k = lc-1;k>=insertindex;k--)
								{
									line[k+1] = line[k];
								}
								line[insertindex] = plist[index2];
								lc++;
							}
						}//end if find
					}//end for int f = 0;f<lc;f++
				}//end if plist[index1].Y == plist[lastindex].Y
				else if(plist[index2].Y == plist[nextindex].Y)
				{
					int insertindex = lc;
					bool ifinsert = true;
					for(int n = 0;n<lc;n++)
					{
						if(fabs(line[n].X-plist[index2].X)<RE && fabs(line[n].Y-plist[index2].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if( plist[index2].X < line[n].X)
						{
							insertindex = n;
							break;
						}
					}//end for int n = 0;n<lc;n++
					if(ifinsert)
					{
						for(int k = lc-1;k>=insertindex;k--)
						{
							line[k+1] = line[k];
						}
						line[insertindex] = plist[index2];
						lc++;
					}
					
					if(!((plist[lastindex].Y > plist[index2].Y && plist[nextindex2].Y > plist[nextindex].Y)
						|| (plist[lastindex].Y < plist[index2].Y && plist[nextindex2].Y < plist[nextindex].Y)))
					{
							int insertindex = c;
							bool ifinsert = true;
							for(int n = 0;n<c;n++)
							{
								if(fabs(parray[n].X-plist[index2].X)<RE && fabs(parray[n].Y-plist[index2].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index2].X < parray[n].X)
								{
									insertindex = n;
									break;
								}
							}
				
							if(ifinsert)
							{
								for(int k = c-1;k>=insertindex;k--)
								{
									parray[k+1] = parray[k];
								}
								parray[insertindex] = plist[index2];
								c++;
							}
					}//end if !(...)
				}//end else if plist[index1].Y == plist[nextindex].Y
				else if ((plist[lastindex] .Y > plist[index2] .Y && plist[nextindex].Y > plist[index2].Y)
					|| (plist[lastindex] .Y < plist[index2] .Y && plist[nextindex].Y < plist[index2].Y) )
				{
					int insertindex = c;
					bool ifinsert = true;
					for(int n = 0;n<c;n++)
					{
						if(fabs(parray[n].X-plist[index2].X)<RE && fabs(parray[n].Y-plist[index2].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if(plist[index2].X < parray[n].X)
						{
							insertindex = n;
							break;
						}
					}
				
					if(ifinsert)
					{
						for(int k = c-1;k>=insertindex;k--)
						{
							parray[k+2] = parray[k];
						}
						parray[insertindex+1] = plist[index2];
						parray[insertindex] = plist[index2];
						c+=2;
					}
				}//end else if (...)
			}//end if nodecount ==2
		}//end else
	}//end for int m = 0;m<PCOUNT;m++
	//两两配对进行栅格化
	for(int g = 0;g<c;g+=2)
	{
		int t1 = (int)parray[g].X/T;
		int t2 = (int)parray[g+1].X/T;
		for(int xx = t1;xx<=t2;xx++)
		{
			result[j*WI+xx] =1;
			if((j-1)*WI+xx>=0)
			{
				result[(j-1)*WI+xx] = 1;
			}
		}
	}
	for(int g = 0;g<lc;g+=2)
	{
		int t1 = (int)line[g].X/T;
		int t2 = (int)line[g+1].X/T;
		for(int xx = t1;xx<=t2;xx++)
		{
			result[j*WI+xx] =1;
			if((j-1)*WI+xx>=0)
			{
				result[(j-1)*WI+xx] = 1;
			}
		}
	}
}
/* Y扫描线
 */
void SetGridValueY(__global MyPoint* plist,int PCOUNT,int i ,float x,int WI,int T,__global int* result)
{
    
	MyPoint parray[100];
	int c = 0;
	MyPoint line[100];
	int lc = 0;
	
	for(int m = 0;m<PCOUNT;m++)
	{
		int mnext = (m+1)%PCOUNT;
		
		MyPoint p1;
		MyPoint p2;
		int index1;
		int index2;
		int nodecount = 0;
		bool ifend = false;
		//重合
		if(fabs(plist[m].X-x) < RE && fabs(plist[mnext].X-x)<RE)
		{
			p1 = plist[m];
			p2 = plist[mnext];
			index1 = m;
			index2 = mnext;
			nodecount =2;
			ifend = true;
		}
		//平行不重合
		else if(fabs(plist[m].X-plist[mnext].X)<RE)
		{
			nodecount = 0;
		}
		//不相交
		else if((x-plist[m].X<-RE && x-plist[mnext].X<-RE)
			|| (x-plist[m].X>RE && x-plist[mnext].X>RE))
		{
			nodecount = 0;
		}
		//过起点
		else if (fabs(plist[m].X -x) < RE)
		{
			p1.X = x; 
			p1.Y = plist[m].Y;
			index1 = m;
			nodecount =1;
			ifend = true;
		}
		//过终点
		else if (fabs(plist[mnext].X -x) < RE)
		{
			p1.X = x;
			p1.Y = plist[mnext].Y;
			index1 = mnext;
			nodecount =1;
			ifend = true;
		}
		//垂直
		else if(fabs(plist[m].Y - plist[mnext].Y) < RE)
		{
			p1.X = x;
			p1.Y = plist[m].Y;
			nodecount =1;
			ifend = false;
		}
		//普通相交
		else
		{
			float xielv = (plist[mnext].Y - plist[m].Y) / (plist[mnext].X - plist[m].X);
            float thisy = (x -plist[m] .X) * xielv + plist[m] .Y;
			p1.X = x;
			p1.Y = thisy;
			nodecount = 1;
			ifend = false;
		}
		//非端点
		if(!ifend)
		{
			if(nodecount >= 1)
			{			
				int insertindex = c;
				bool ifinsert = true;
				for(int n = 0;n<c;n++)
				{
					if(fabs(parray[n].X-p1.X)<RE && fabs(parray[n].Y-p1.Y)<RE)
					{
						ifinsert = false;
						break;
					}
					if( p1.Y < parray[n].Y)
					{
						insertindex = n;
						break;
					}
				}
				
				if(ifinsert)
				{
					for(int k = c-1;k>=insertindex;k--)
					{
						parray[k+1] = parray[k];
					}
					parray[insertindex] = p1;
					c++;
				}
			}//end if nodecount >=1
			if(nodecount == 2)
			{			
				int insertindex = c;
				bool ifinsert = true;
				for(int n = 0;n<c;n++)
				{
					if(fabs(parray[n].X-p2.X)<RE && fabs(parray[n].Y-p2.Y)<RE)
					{
						ifinsert = false;
						break;
					}
					if( p2.Y < parray[n].Y)
					{
						insertindex = n;
						break;
					}
				}
				
				if(ifinsert)
				{
					for(int k = c-1;k>=insertindex;k--)
					{
						parray[k+1] = parray[k];
					}
					parray[insertindex] = p2;
					c++;
				}
			}//end if nodecount == 2
		}//end if !ifend
		//端点
		else
		{
			if(nodecount >=1)
			{
				int lastindex = (index1-1+PCOUNT)%PCOUNT;
				int nextindex = (index1+1)%PCOUNT;
				int nextindex2 = (index1+1)%PCOUNT;
				if(plist[index1].X == plist[lastindex].X)
				{
					bool find = false;
					for(int f = 0;f<lc;f++)
					{
						if(line[f].X == plist[index1].X && line[f].Y == plist[index1].Y)
						{
							find = true;
							break;
						}
						if(find)
						{
							int insertindex = lc;
							bool ifinsert = true;
							for(int n = 0;n<lc;n++)
							{
								if(fabs(line[n].X-plist[index1].X)<RE && fabs(line[n].Y-plist[index1].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index1].Y < line[n].Y)
								{
									insertindex = n;
									break;
								}
							}//end for int n = 0;n<lc;n++
							if(ifinsert)
							{
								for(int k = lc-1;k>=insertindex;k--)
								{
									line[k+1] = line[k];
								}
								line[insertindex] = plist[index1];
								lc++;
							}
						}//end if find
					}//end for int f = 0;f<lc;f++
				}//end if plist[index1].Y == plist[lastindex].Y
				else if(plist[index1].X == plist[nextindex].X)
				{
					int insertindex = lc;
					bool ifinsert = true;
					for(int n = 0;n<lc;n++)
					{
						if(fabs(line[n].X-plist[index1].X)<RE && fabs(line[n].Y-plist[index1].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if( plist[index1].Y < line[n].Y)
						{
							insertindex = n;
							break;
						}
					}//end for int n = 0;n<lc;n++
					if(ifinsert)
					{
						for(int k = lc-1;k>=insertindex;k--)
						{
							line[k+1] = line[k];
						}
						line[insertindex] = plist[index1];
						lc++;
					}
					
					if(!((plist[lastindex].X > plist[index1].X && plist[nextindex2].X > plist[nextindex].X)
						|| (plist[lastindex].X < plist[index1].X && plist[nextindex2].X < plist[nextindex].X)))
					{
							int insertindex = c;
							bool ifinsert = true;
							for(int n = 0;n<c;n++)
							{
								if(fabs(parray[n].X-plist[index1].X)<RE && fabs(parray[n].Y-plist[index1].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index1].Y < parray[n].Y)
								{
									insertindex = n;
									break;
								}
							}
				
							if(ifinsert)
							{
								for(int k = c-1;k>=insertindex;k--)
								{
									parray[k+1] = parray[k];
								}
								parray[insertindex] = plist[index1];
								c++;
							}
					}//end if !(...)
				}//end else if plist[index1].Y == plist[nextindex].Y
				else if ((plist[lastindex] .X > plist[index1] .X && plist[nextindex].X > plist[index1].X)
					|| (plist[lastindex] .X < plist[index1] .X && plist[nextindex].X < plist[index1].X) )
				{
					int insertindex = c;
					bool ifinsert = true;
					for(int n = 0;n<c;n++)
					{
						if(fabs(parray[n].X-plist[index1].X)<RE && fabs(parray[n].Y-plist[index1].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if(plist[index1].Y < parray[n].Y)
						{
							insertindex = n;
							break;
						}
					}
				
					if(ifinsert)
					{
						for(int k = c-1;k>=insertindex;k--)
						{
							parray[k+2] = parray[k];
						}
						parray[insertindex+1] = plist[index1];
						parray[insertindex] = plist[index1];
						c+=2;
					}
				}//end else if (...)
			}//end if nodecount >=1
			if (nodecount ==2)
			{
				int lastindex = (index2-1+PCOUNT)%PCOUNT;
				int nextindex = (index2+1)%PCOUNT;
				int nextindex2 = (index2+1)%PCOUNT;
				if(plist[index2].X == plist[lastindex].X)
				{
					bool find = false;
					for(int f = 0;f<lc;f++)
					{
						if(line[f].X == plist[index2].X && line[f].Y == plist[index2].Y)
						{
							find = true;
							break;
						}
						if(find)
						{
							int insertindex = lc;
							bool ifinsert = true;
							for(int n = 0;n<lc;n++)
							{
								if(fabs(line[n].X-plist[index2].X)<RE && fabs(line[n].Y-plist[index2].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index2].Y < line[n].Y)
								{
									insertindex = n;
									break;
								}
							}//end for int n = 0;n<lc;n++
							if(ifinsert)
							{
								for(int k = lc-1;k>=insertindex;k--)
								{
									line[k+1] = line[k];
								}
								line[insertindex] = plist[index2];
								lc++;
							}
						}//end if find
					}//end for int f = 0;f<lc;f++
				}//end if plist[index1].Y == plist[lastindex].Y
				else if(plist[index2].X == plist[nextindex].X)
				{
					int insertindex = lc;
					bool ifinsert = true;
					for(int n = 0;n<lc;n++)
					{
						if(fabs(line[n].X-plist[index2].X)<RE && fabs(line[n].Y-plist[index2].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if( plist[index2].Y < line[n].Y)
						{
							insertindex = n;
							break;
						}
					}//end for int n = 0;n<lc;n++
					if(ifinsert)
					{
						for(int k = lc-1;k>=insertindex;k--)
						{
							line[k+1] = line[k];
						}
						line[insertindex] = plist[index2];
						lc++;
					}
					
					if(!((plist[lastindex].X > plist[index2].X && plist[nextindex2].X > plist[nextindex].X)
						|| (plist[lastindex].X < plist[index2].X && plist[nextindex2].X < plist[nextindex].X)))
					{
							int insertindex = c;
							bool ifinsert = true;
							for(int n = 0;n<c;n++)
							{
								if(fabs(parray[n].X-plist[index2].X)<RE && fabs(parray[n].Y-plist[index2].Y)<RE)
								{
									ifinsert = false;
									break;
								}
								if( plist[index2].Y < parray[n].Y)
								{
									insertindex = n;
									break;
								}
							}
				
							if(ifinsert)
							{
								for(int k = c-1;k>=insertindex;k--)
								{
									parray[k+1] = parray[k];
								}
								parray[insertindex] = plist[index2];
								c++;
							}
					}//end if !(...)
				}//end else if plist[index1].Y == plist[nextindex].Y
				else if ((plist[lastindex] .X > plist[index2] .X && plist[nextindex].X > plist[index2].X)
					|| (plist[lastindex] .X < plist[index2] .X && plist[nextindex].X < plist[index2].X) )
				{
					int insertindex = c;
					bool ifinsert = true;
					for(int n = 0;n<c;n++)
					{
						if(fabs(parray[n].X-plist[index2].X)<RE && fabs(parray[n].Y-plist[index2].Y)<RE)
						{
							ifinsert = false;
							break;
						}
						if(plist[index2].Y < parray[n].Y)
						{
							insertindex = n;
							break;
						}
					}
				
					if(ifinsert)
					{
						for(int k = c-1;k>=insertindex;k--)
						{
							parray[k+2] = parray[k];
						}
						parray[insertindex+1] = plist[index2];
						parray[insertindex] = plist[index2];
						c+=2;
					}
				}//end else if (...)
			}//end if nodecount ==2
		}//end else
	}//end for int m = 0;m<PCOUNT;m++
	//两两配对进行栅格化
	for(int g = 0;g<c;g+=2)
	{
		int t1 = (int)parray[g].Y/T;
		int t2 = (int)parray[g+1].Y/T;
		for(int yy = t1;yy<=t2;yy++)
		{
			result[yy*WI+i] =1;
			if(i-1>=0)
			{
				result[yy*WI+i-1] =1;
			}
		}
	}
	for(int g = 0;g<lc;g+=2)
	{
		int t1 = (int)line[g].Y/T;
		int t2 = (int)line[g+1].Y/T;
		for(int yy = t1;yy<=t2;yy++)
		{
			result[yy*WI+i] =1;
			if(i-1>=0)
			{
				result[yy*WI+i-1] = 1;
			}
		}
	}
}



__kernel void GetGridValue(__global MyPoint* plist,int pcount,float W,float H,float T,int WI,int HI,__global int* result)
{
	int i = get_global_id(0);
    if(i < HI)
    {
        int y = i*T;
        if(y > H)
        {
            y = H;
        }
        SetGridValueX(plist,pcount,i,y,WI,T,result);
    }
    else
    {
        int j = i-HI;
        int x = j*T;
        if(x > W)
	    {
		    x = W;
	    }
        SetGridValueY(plist,pcount,j,x,WI,T,result);
    }
}

__kernel void InitArray(__global int* result)
{
	result[get_global_id(0)] = 0;
}


__kernel void Insert(__global const int* test1,__global const int *test2 ,__global int *result)
{
    unsigned int id = get_global_id(0); 
	result[id] = test1[id] + test2[id];
}